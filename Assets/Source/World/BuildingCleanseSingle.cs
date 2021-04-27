using UnityEngine;

public class BuildingCleanseSingle : Building
{

    public override void TickCorruption()
    {
        if (!IsOperable())
            return;

        BuildingAssetCleanseSingle asset = (BuildingAssetCleanseSingle)m_Cell.m_BuildingAsset;

        // Remove corruption on single cell
        m_Cell.m_CorruptionDelta -= asset.m_CorruptionRemoval;
    }

}
