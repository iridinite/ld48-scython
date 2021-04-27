using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LayerInfoController : MonoBehaviour
{

    public TMP_Text m_TextLayerNo;
    public TMP_Text m_TextAutoDescentTime;
    public TMP_Text m_TextManualDescent;
    public TMP_Text m_TextManualDescentAward;

    public Button m_ManualDescendButton;
    public GameObject m_ManualDescendTooltip;

    private GameController m_Ctrl;

    private void Awake()
    {
        m_Ctrl = FindObjectOfType<GameController>();
    }

    private void FixedUpdate()
    {
        int layer = GameSession.Instance.PortalStage;
        if (layer == 0)
        {
            m_TextLayerNo.text = $"Portal Closed";
            m_TextManualDescent.text = "Open the Portal";
            m_TextAutoDescentTime.text = "When you are ready...";
            m_ManualDescendButton.interactable = true;
            m_ManualDescendTooltip.SetActive(false);
        }
        else
        {
            m_TextManualDescent.text = "Descend Now";
            m_TextLayerNo.text = $"Layer <font=\"Zebulon\">{layer}";
            m_TextAutoDescentTime.text = $"{m_Ctrl.GetTimeUntilAutoAdvance():F0} seconds until descent";
            m_TextManualDescentAward.text = $"Descend early to\nreceive <sprite name=Infernite> {GetManualAdvanceBonus()}";
            m_ManualDescendButton.interactable = GameSession.Instance.IsManualAdvanceAllowed;
        }
    }

    private int GetManualAdvanceBonus()
    {
        // No award for layer 0, duh
        if (GameSession.Instance.PortalStage == 0)
            return 0;

        // Percentage of remaining siphon output for this stage
        float remaining = m_Ctrl.GetTimeUntilAutoAdvance() / Tuning.k_CorruptionSpreadInterval * GameSession.Instance.PortalLayer.m_MoneyIncomePerSiphon;
        float adjusted = remaining * Tuning.k_ManualAdvanceBonusFactor;
        return Mathf.CeilToInt(adjusted);
    }

    public void OnManualDescend()
    {
        // Award early-descent bonus
        int award = GetManualAdvanceBonus();
        Debug.Log($"Manual descent, awarding {award} money");
        GameSession.Instance.Money += award;

        // Hide tooltip
        m_ManualDescendTooltip.SetActive(false);

        // Increase stage number
        m_Ctrl.AdvancePortalStage();
    }

    public void OnMouseEnterManualDescend()
    {
        if (m_ManualDescendButton.interactable && GameSession.Instance.PortalStage != 0)
            m_ManualDescendTooltip.SetActive(true);
    }

    public void OnMouseExitManualDescend()
    {
        m_ManualDescendTooltip.SetActive(false);
    }

}
