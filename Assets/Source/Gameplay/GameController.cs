using UnityEngine;

public class GameController : MonoBehaviour
{

    private float m_UpkeepTimer;
    private float m_AutoAdvanceTimer;
    private float m_ManualAdvanceTimer;
    private float m_SpreadTimer;
    private float m_SpreadActivationTimer;
    private bool m_EnergyRefreshRequired;
    private bool m_GameEnded;

    private Map m_Map;

    [Header("Gameplay")]
    public PortalLayerAsset[] m_TheLayersOfHell;

    [Header("System")]
    public GameEndFadeController m_FadeController;
    public MusicController m_MusicController;

    [Header("Triggers")]
    public GameObject m_LayerDescendTrigger;

    private void Awake()
    {
        m_UpkeepTimer = Tuning.k_UpkeepInterval;
        m_SpreadTimer = Tuning.k_CorruptionSpreadInterval;
        m_GameEnded = false;
        m_Map = FindObjectOfType<Map>();

        // this codebase is getting a bit messy now
        GameSession.Instance.PortalStageCount = m_TheLayersOfHell.Length;
    }

    private void Update()
    {
        if (m_GameEnded)
            return;

        // Most game logic is inactive if the portal is closed
        if (GameSession.Instance.PortalStage != 0)
        {
            // Take upkeep
            m_UpkeepTimer -= Time.deltaTime;
            if (m_UpkeepTimer <= 0.0f)
            {
                m_UpkeepTimer = Tuning.k_UpkeepInterval;
                RunUpkeep();
            }

            // Toggle spread activation flag whenever the timer expires
            m_SpreadActivationTimer -= Time.deltaTime;
            if (m_SpreadActivationTimer <= 0.0f)
            {
                var layer = GameSession.Instance.PortalLayer;
                GameSession.Instance.IsCorruptionWaveActive = !GameSession.Instance.IsCorruptionWaveActive;
                m_SpreadActivationTimer = GameSession.Instance.IsCorruptionWaveActive
                    ? layer.m_WaveDuration
                    : layer.m_WaveQuietPeriod;
            }

            // If spreading is currently allowed (wave is active), update that
            m_SpreadTimer -= Time.deltaTime;
            if (m_SpreadTimer <= 0.0f)
            {
                m_SpreadTimer = Tuning.k_CorruptionSpreadInterval;
                RunCorruption();
            }

            // Manage the auto/manual-advance timers
            m_ManualAdvanceTimer -= Time.deltaTime;
            if (m_ManualAdvanceTimer <= 0.0f)
                GameSession.Instance.IsManualAdvanceAllowed = true;
            m_AutoAdvanceTimer -= Time.deltaTime;
            if (m_AutoAdvanceTimer <= 0.0f)
                AdvancePortalStage();
        }

        // Energy network refresh
        if (m_EnergyRefreshRequired)
        {
            m_EnergyRefreshRequired = false;
            EnergyUtil.RefreshEnergyNetwork(m_Map);
        }
    }

    public float GetTimeUntilAutoAdvance()
    {
        return m_AutoAdvanceTimer;
    }

    public float GetTimeUntilWaveToggle()
    {
        return m_SpreadActivationTimer;
    }

    public void RequestEnergyNetworkRefresh()
    {
        m_EnergyRefreshRequired = true;
    }

    private void RunUpkeep()
    {
        bool hasSiphons = false;

        // Sum the total upkeep of all cells in the map
        int upkeep = 0;
        for (int y = 0; y < Tuning.k_MapSize; y++)
            for (int x = 0; x < Tuning.k_MapSize; x++)
            {
                if (!m_Map.IsValid(x, y))
                    continue;

                var cell = m_Map.GetCellAt(x, y);
                var asset = cell.m_BuildingAsset;
                upkeep += asset.m_UpkeepCost;

                // bit of a hack to do this here, since it has nothing to do with upkeep, but I don't want to do ANOTHER iteration
                if (cell.m_BuildingInstance is BuildingSiphon siphon && siphon.IsOperable())
                    hasSiphons = true;
            }

        // Apply the deltas
        GameSession.Instance.Money -= upkeep;

        // Game-over if all siphons are broken
        if (!hasSiphons)
        {
            Debug.Log("Game lost");
            if (!m_GameEnded)
            {
                m_FadeController.FadeToScene("L_GameLose");
                m_MusicController.RequestFadeOut();
            }

            m_GameEnded = true; // Disable game logic
        }
    }

    private void RunCorruption()
    {
        // First pass: run callbacks to create or remove corruption
        for (int y = 0; y < Tuning.k_MapSize; y++)
            for (int x = 0; x < Tuning.k_MapSize; x++)
                m_Map.GetCellAt(x, y)?.m_BuildingInstance.TickCorruption();

        // Second pass: spread corruption
        for (int y = 0; y < Tuning.k_MapSize; y++)
            for (int x = 0; x < Tuning.k_MapSize; x++)
            {
                // If this cell is eligible for spreading, do that now
                GridCell cell = m_Map.GetCellAt(x, y);
                if (cell != null && cell.m_Corruption >= Tuning.k_CorruptionSpreadThreshold)
                    RunCorruptionOnCell(cell);
            }

        // Third pass: apply all calculated deltas. This must be done separately so one cell's spread doesn't influence the neighbors' spread
        for (int y = 0; y < Tuning.k_MapSize; y++)
            for (int x = 0; x < Tuning.k_MapSize; x++)
                m_Map.GetCellAt(x, y)?.ApplyCorruption();

        // Refresh the power network now that tiles may have gained/lost corruption
        RequestEnergyNetworkRefresh();
    }

    private void RunCorruptionOnCell(GridCell cell)
    {
        // Add it to all neighbors
        var pos = cell.m_MapPos;
        m_Map.GetCellAt(pos.x - 1, pos.y)?.SpreadCorruptionHere(cell);
        m_Map.GetCellAt(pos.x + 1, pos.y)?.SpreadCorruptionHere(cell);
        m_Map.GetCellAt(pos.x, pos.y - 1)?.SpreadCorruptionHere(cell);
        m_Map.GetCellAt(pos.x, pos.y + 1)?.SpreadCorruptionHere(cell);
    }

    public void AdvancePortalStage()
    {
        // All layers done -> win
        if (GameSession.Instance.PortalStage == m_TheLayersOfHell.Length)
        {
            Debug.Log("Game won");
            if (!m_GameEnded)
            {
                m_MusicController.RequestFadeOut();
                m_FadeController.FadeToScene("L_GameWin");
            }

            m_GameEnded = true; // Disable game logic
            return;
        }

        // Select the next layer
        GameSession.Instance.PortalLayer = m_TheLayersOfHell[GameSession.Instance.PortalStage];
        GameSession.Instance.PortalStage++;

        // Music change
        if (GameSession.Instance.PortalStage == 6)
            m_MusicController.RequestLateGame();

        // Debug log
        var layer = GameSession.Instance.PortalLayer;
        Debug.Log("Now using layer: " + layer.name, layer);

        // Apply new timers
        GameSession.Instance.IsManualAdvanceAllowed = false;
        m_AutoAdvanceTimer = layer.m_TimeUntilAutoAdvance;
        m_ManualAdvanceTimer = layer.m_TimeUntilManualAdvance;

        // Disable spread with the initial delay
        GameSession.Instance.IsCorruptionWaveActive = false;
        m_SpreadActivationTimer = layer.m_CorruptionInitialDelay;

        // Spooky sound
        if (m_LayerDescendTrigger != null)
            Instantiate(m_LayerDescendTrigger);
    }

}
