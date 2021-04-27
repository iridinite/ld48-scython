using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/Cleanse Area")]
public class BuildingAssetCleanseTower : BuildingAssetPowerConsumer
{

    [Header("Cleansing Tower")]
    [Min(0)] public int m_Range;
    public float m_CorruptionRemoval;

}
