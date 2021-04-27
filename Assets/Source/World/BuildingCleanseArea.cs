using UnityEngine;

public class BuildingCleanseArea : BuildingPowerConsumer
{

    public override void TickCorruption()
    {
        if (!IsOperable() || !IsPowered())
            return;

        BuildingAssetCleanseTower asset = (BuildingAssetCleanseTower)m_Cell.m_BuildingAsset;

        // Cleanse tiles in a circular area
        var source = m_Cell.m_MapPos;
        for (int y = -asset.m_Range; y <= asset.m_Range; y++)
            for (int x = -asset.m_Range; x <= asset.m_Range; x++)
            {
                // Calculate target coordinates
                var target = source + new Vector2Int(x, y);
                if (!m_Map.IsValid(target))
                    continue;

                // Apply AoE cleanse to the target tile
                var cell = m_Map.GetCellAt(target);
                cell.m_CorruptionDelta -= asset.m_CorruptionRemoval;
            }
    }

}
