using TMPro;
using UnityEngine;

public class BuildingTooltipController : MonoBehaviour
{

    public TMP_Text m_Text;
    public GameObject m_Frame;

    private void Awake()
    {
        HideTooltip();
    }

    public void ShowTooltip(string text)
    {
        m_Text.text = text;
        m_Frame.SetActive(true);
    }

    public void HideTooltip()
    {
        m_Frame.SetActive(false);
    }

}
