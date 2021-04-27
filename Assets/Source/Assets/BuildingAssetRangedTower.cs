using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/Ranged")]
public class BuildingAssetRangedTower : BuildingAssetPowerConsumer
{

    [Header("Ranged Tower")]
    [Min(0)] public int m_Range;
    public float m_CorruptionRemoval;

}
