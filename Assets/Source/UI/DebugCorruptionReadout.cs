using TMPro;
using UnityEngine;

public class DebugCorruptionReadout : MonoBehaviour
{

    public TMP_Text m_Text;
    public GameObject m_VisualRoot;

    private Building m_Building;
    private MapCursor m_Cursor;

    private void Start()
    {
        m_Building = GetComponentInParent<Building>();
        m_Cursor = FindObjectOfType<MapCursor>();
    }

    private void FixedUpdate()
    {
        if (m_Cursor.GetHoverPosition() == m_Building.m_Cell.m_MapPos // hovering?
            && (m_Building.m_Cell.m_Corruption >= 1f || m_Building is BuildingPowerGenerator)) // have something to show?
        {
            // Hovering over this tile, display a readout
            m_VisualRoot.SetActive(true);
            string readout = $"<sprite name=Corruption> {m_Building.m_Cell.m_Corruption:F0}";
            if (m_Building is BuildingPowerGenerator generator)
                readout += $"\n<sprite name=Energy> {generator.m_PowerAvailable}";
            m_Text.text = readout;
        }
        else
        {
            // Hide readout
            m_VisualRoot.SetActive(false);
        }
    }

}
