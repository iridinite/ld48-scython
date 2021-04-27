using UnityEngine;

public class BuildingCable : Building
{

    public GameObject m_CablePart_N;
    public GameObject m_CablePart_S;
    public GameObject m_CablePart_E;
    public GameObject m_CablePart_W;

    public override void ResetPower()
    {
        // Look for neighbors
        CheckNeighbor(m_Cell.m_MapPos + Vector2Int.up, m_CablePart_W);
        CheckNeighbor(m_Cell.m_MapPos + Vector2Int.down, m_CablePart_E);
        CheckNeighbor(m_Cell.m_MapPos + Vector2Int.left, m_CablePart_S);
        CheckNeighbor(m_Cell.m_MapPos + Vector2Int.right, m_CablePart_N);
    }

    private void CheckNeighbor(Vector2Int pos, GameObject cable)
    {
        var cell = m_Map.GetCellAt(pos);
        bool canConnect = cell != null && (
            cell.m_BuildingAsset is BuildingAssetCable || 
            cell.m_BuildingAsset is BuildingAssetPowerGenerator ||
            (!(this is BuildingPowerConsumer) && cell.m_BuildingAsset is BuildingAssetPowerConsumer));

        cable.SetActive(canConnect);
    }

}
