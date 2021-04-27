using UnityEngine;

public class PlacementHintController : MonoBehaviour
{

    public GameObject m_Toggle;

    private MapCursor m_Cursor;

    private void Awake()
    {
        m_Cursor = FindObjectOfType<MapCursor>();
    }

    private void FixedUpdate()
    {
        m_Toggle.SetActive(m_Cursor.IsPlacementActive());
    }

}
