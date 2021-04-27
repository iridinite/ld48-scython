using UnityEngine;

[CreateAssetMenu(menuName = "Portal Layer")]
public class PortalLayerAsset : ScriptableObject
{

    [Header("The Good")]
    [Min(0)] public int m_MoneyIncomePerSiphon;
    [Min(0)] public float m_TimeUntilAutoAdvance;
    [Min(0)] public float m_TimeUntilManualAdvance;

    [Header("The Bad")]
    [Min(0)] public float m_CorruptionOutput;
    [Min(0)] public float m_CorruptionInitialDelay;
    [Min(0)] public float m_WaveDuration;
    [Min(0)] public float m_WaveQuietPeriod;

}
