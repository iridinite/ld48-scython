using UnityEngine;

public class GridCell
{

    public float m_Corruption = 0.0f;
    public float m_CorruptionDelta = 0.0f;
    public Vector2Int m_MapPos;

    public GameObject m_ModelInstance;
    public Building m_BuildingInstance;
    public BuildingAsset m_BuildingAsset;

    public void SetBuilding(Transform container, BuildingAsset asset)
    {
        if (m_ModelInstance != null)
            Object.Destroy(m_ModelInstance);

        var worldpos = Map.MapToWorld(m_MapPos);
        m_ModelInstance = Object.Instantiate(asset.m_Prefab, worldpos, Quaternion.identity, container);

        m_BuildingAsset = asset;
        m_BuildingInstance = m_ModelInstance.GetComponent<Building>();
        m_BuildingInstance.m_Cell = this;
    }

    public void SpreadCorruptionHere(GridCell source)
    {
        // Calculate delta-spread
        float difference = source.m_Corruption - (this.m_Corruption + this.m_BuildingAsset.m_CorruptionSaturation);
        if (difference <= Tuning.k_CorruptionEquilibriumThreshold)
            return;

        // Add corruption here
        float desired_delta = difference * Tuning.k_CorruptionSpreadFactor;
        this.m_CorruptionDelta += desired_delta;

        // Remove transferred corruption from the source
        source.m_CorruptionDelta -= desired_delta;
    }

    public void ApplyCorruption()
    {
        m_Corruption = Mathf.Max(m_Corruption + m_CorruptionDelta, 0.0f);
        m_CorruptionDelta = 0.0f;
    }

}
