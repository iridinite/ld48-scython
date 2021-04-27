using System.Collections.Generic;
using UnityEngine;

public class BuildingRangedTower : BuildingPowerConsumer
{

    public LineRenderer m_LineRenderer;
    public Transform m_LineAnchor;

    private void Start()
    {
        // Hide initially, and fix anchor to self
        m_LineRenderer.SetPosition(0, m_LineAnchor.position);
        m_LineRenderer.gameObject.SetActive(false);
    }

    public override void TickCorruption()
    {
        if (!IsOperable() || !IsPowered())
            return;

        BuildingAssetRangedTower asset = (BuildingAssetRangedTower)m_Cell.m_BuildingAsset;

        // Find tiles that have corruption on them
        var local = m_Cell.m_MapPos;
        var heretics = new List<GridCell>(9);
        for (int y = local.y - asset.m_Range; y <= local.y + asset.m_Range; y++)
            for (int x = local.x - asset.m_Range; x <= local.x + asset.m_Range; x++)
                if (m_Map.IsValid(x, y) && m_Map.GetCellAt(x, y).m_Corruption > 0.0f)
                    heretics.Add(m_Map.GetCellAt(x, y));

        // No work to do if no tiles nearby
        if (heretics.Count == 0)
        {
            m_LineRenderer.gameObject.SetActive(false);
            return;
        }

        // Find the closest one
        heretics.Sort((lhs, rhs) => Comparer<float>.Default.Compare(
            Vector2Int.Distance(lhs.m_MapPos, local),
            Vector2Int.Distance(rhs.m_MapPos, local))
        );

        // Remove corruption from it
        heretics[0].m_CorruptionDelta -= asset.m_CorruptionRemoval;

        // Update rendering
        m_LineRenderer.gameObject.SetActive(true);
        m_LineRenderer.SetPosition(1, Map.MapToWorld(heretics[0].m_MapPos));
    }

}
