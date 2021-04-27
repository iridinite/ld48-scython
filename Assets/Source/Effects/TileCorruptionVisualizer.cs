using UnityEngine;

public class TileCorruptionVisualizer : MonoBehaviour
{

    [Header("Animation")]
    public GameObject m_VisualRoot;
    public GameObject m_InoperableMark;
    public GameObject m_NoPowerMark;
    public GameObject m_VFXRoot;
    public ParticleSystem m_AmbientParticles;
    public AnimationCurve m_AmbientParticleIntensity;

    [Header("Floor")]
    public AnimationCurve m_FloorCurve;
    public Renderer m_FloorRenderer;

    [Header("Triggers")]
    public GameObject m_On50PctReached;
    public GameObject m_On100PctReached;
    public GameObject m_OnDisabled;

    private Building m_Building;
    private Transform m_Transform;
    private float m_AmbientMaxEmission;
    private float m_LastKnownCorruption;
    private bool m_WasDisabled;
    private MaterialPropertyBlock m_FloorProperties;

    private void Awake()
    {
        m_Building = GetComponentInParent<Building>();
        m_Transform = GetComponent<Transform>();
        m_AmbientMaxEmission = m_AmbientParticles.emission.rateOverTime.constant;
        m_FloorProperties = new MaterialPropertyBlock();
        m_WasDisabled = false;
    }

    private void FixedUpdate()
    {
        var value = m_Building.m_Cell.m_Corruption;
        var factor = value / Tuning.k_CorruptionMax;

        // Disable particles outright if corruption is too low
        bool show = value >= Tuning.k_CorruptionVisualThreshold;
        m_VisualRoot.SetActive(show);

        // Check for power
        bool powered = true;
        if (m_Building is BuildingPowerConsumer consumer)
            powered = consumer.m_IsPowered;
        m_NoPowerMark.SetActive(!powered);

        // Apply texture lerp for floor
        m_FloorProperties.SetFloat("T", m_FloorCurve.Evaluate(factor));
        m_FloorRenderer.SetPropertyBlock(m_FloorProperties);

        if (show)
        {
            // Portal has these animations disabled because it's always 100% corrupted
            if (m_Building.m_Cell.m_BuildingAsset.m_ShowCorruptionAnimations)
            {
                // 50% reached
                if (m_On50PctReached != null && value >= Tuning.k_CorruptionSpreadThreshold &&
                    m_LastKnownCorruption < Tuning.k_CorruptionSpreadThreshold)
                    Instantiate(m_On50PctReached, m_Transform.position, Quaternion.identity);

                // 100% reached
                if (m_On100PctReached != null && value >= Tuning.k_CorruptionMax &&
                    m_LastKnownCorruption < Tuning.k_CorruptionMax)
                    Instantiate(m_On100PctReached, m_Transform.position, Quaternion.identity);
            }

            // Update inoperable mark (and do not show both the inoperable and no-power marks)
            //m_InoperableMark.SetActive(!m_Building.IsOperable() && powered);

            // Apply emission rate
            var emission = m_AmbientParticles.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(m_AmbientParticleIntensity.Evaluate(factor) * m_AmbientMaxEmission);
        }

        // VFX toggle
        bool showVfx = powered && m_Building.IsOperable() && GameSession.Instance.PortalStage != 0;
        m_VFXRoot.SetActive(showVfx);

        if (showVfx)
            m_WasDisabled = false;

        if (!m_WasDisabled && !showVfx && m_Building.m_Cell.m_BuildingAsset.m_CanBeInoperable && GameSession.Instance.PortalStage != 0)
        {
            Debug.Log("Spawning nopower sound");
            if (m_OnDisabled != null)
                Instantiate(m_OnDisabled, Vector3.zero, Quaternion.identity);
            m_WasDisabled = true;
        }

        m_LastKnownCorruption = value;
    }

}
