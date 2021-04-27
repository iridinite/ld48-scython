using System;
using UnityEngine;

public class Building : MonoBehaviour
{

    [NonSerialized] public GridCell m_Cell;
    [NonSerialized] public Map m_Map;

    public Renderer[] m_CorruptableRenderers;

    private void Awake()
    {
        m_Map = FindObjectOfType<Map>();
    }

    public bool IsOperable()
    {
        // Some buildings cannot become inoperable
        if (!m_Cell.m_BuildingAsset.m_CanBeInoperable)
            return true;

        // Is not fully corrupted?
        return m_Cell.m_Corruption < Tuning.k_CorruptionMax;
    }

    private void LateUpdate()
    {
        var corruption = IsOperable() ? 0f : 1f;
        foreach (var r in m_CorruptableRenderers)
        {
            var props = new MaterialPropertyBlock();
            props.SetFloat("Corruption", corruption);
            r.SetPropertyBlock(props);
        }
    }

    public virtual void ResetPower() {}

    public virtual void TickCorruption() {}

}
