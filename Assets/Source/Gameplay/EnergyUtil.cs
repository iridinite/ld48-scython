using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnergyUtil
{

    public static void RefreshEnergyNetwork(Map map)
    {
        // Reset power output of all generators
        for (int y = 0; y < Tuning.k_MapSize; y++)
            for (int x = 0; x < Tuning.k_MapSize; x++)
                map.GetCellAt(x, y)?.m_BuildingInstance.ResetPower();

        // Then reevaluate the power state of consumers
        for (int y = 0; y < Tuning.k_MapSize; y++)
            for (int x = 0; x < Tuning.k_MapSize; x++)
                RefreshEnergyNetworkOfCell(map, map.GetCellAt(x, y));
    }

    public static bool CanBuildingSpreadEnergy(BuildingAsset asset)
    {
        return asset is BuildingAssetCable || asset is BuildingAssetPowerGenerator;
    }

    private static void RefreshEnergyNetworkOfCell(Map map, GridCell origin)
    {
        // Only run this logic for consumers
        if (!(origin?.m_BuildingAsset is BuildingAssetPowerConsumer consumer))
            return;

        // Skip consumers that are inoperable due to corruption
        if (!origin.m_BuildingInstance.IsOperable())
            return;

        // Flood fill outward to find all nearby generators
        var generators = new List<BuildingPowerGenerator>();
        var visited = new HashSet<Vector2Int>();
        var cables = new Stack<Vector2Int>(16);
        cables.Push(origin.m_MapPos);
        while (cables.Count > 0)
        {
            // Get the next tile to visit
            Vector2Int visit = cables.Pop();
            visited.Add(visit);

            // Get state of this tile
            GridCell cell = map.GetCellAt(visit);
            BuildingAsset asset = cell.m_BuildingAsset;

            // Find generators
            if (asset is BuildingAssetPowerGenerator)
                generators.Add((BuildingPowerGenerator)cell.m_BuildingInstance);

            // Visit neighbors only if this is a cable or this is the origin site
            if (CanBuildingSpreadEnergy(asset) || visit == origin.m_MapPos)
            {
                TryVisit(map, visit + Vector2Int.up, cables, visited);
                TryVisit(map, visit + Vector2Int.down, cables, visited);
                TryVisit(map, visit + Vector2Int.left, cables, visited);
                TryVisit(map, visit + Vector2Int.right, cables, visited);
            }
        }

        // Now determine which generators to draw power from.
        int total_available = generators.Sum(generator => generator.m_PowerAvailable);
        if (total_available < consumer.m_EnergyConsumption)
        {
            // Not enough power is available, power remains off
            //Debug.Log($"Consumer {origin.m_MapPos} ({origin.m_BuildingAsset.name}) is UNPOWERED");
            return;
        }

        // Remove energy from generators so other consumers cannot claim it
        int total_draw = consumer.m_EnergyConsumption;
        foreach (var generator in generators)
        {
            int local_draw = Mathf.Min(total_draw, generator.m_PowerAvailable);
            generator.m_PowerAvailable -= local_draw;
            total_draw -= local_draw;
            //Debug.Log($"Drawing {local_draw} from generator at {generator.m_Cell.m_MapPos}, {generator.m_PowerAvailable} remaining");
        }

        // This building is now considered powered
        ((BuildingPowerConsumer)origin.m_BuildingInstance).m_IsPowered = true;
        //Debug.Log($"Consumer {origin.m_MapPos} ({origin.m_BuildingAsset.name}) is powered");
    }

    private static void TryVisit(Map map, Vector2Int pos, Stack<Vector2Int> stack, HashSet<Vector2Int> visited)
    {
        if (visited.Contains(pos))
            return;

        if (!map.IsValid(pos))
            return;

        stack.Push(pos);
    }

}
