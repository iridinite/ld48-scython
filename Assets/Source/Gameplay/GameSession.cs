public class GameSession
{

    public static GameSession Instance { get; set; } = new GameSession();

    public int Money { get; set; } = Tuning.k_StartingMoney;

    public int PortalStage { get; set; } = 0;

    public int PortalStageCount { get; set; } = 1;

    public PortalLayerAsset PortalLayer { get; set; }

    public bool IsManualAdvanceAllowed { get; set; }

    public bool IsCorruptionWaveActive { get; set; }

}
