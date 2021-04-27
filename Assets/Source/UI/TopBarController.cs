using TMPro;
using UnityEngine;

public class TopBarController : MonoBehaviour
{

    public TMP_Text m_TextMoney;
    public TMP_Text m_TextDebugWaveTimer;

    private void Start()
    {
        m_TextDebugWaveTimer.gameObject.SetActive(false);
    }

    private void Update()
    {
        m_TextMoney.text = $"<sprite name=Infernite> {GameSession.Instance.Money}";

#if UNITY_EDITOR
        // debug readout
        var ctrl = FindObjectOfType<GameController>();
        m_TextDebugWaveTimer.gameObject.SetActive(true);
        m_TextDebugWaveTimer.text =
            $"Wave {(GameSession.Instance.IsCorruptionWaveActive ? "stops" : "starts")} in {ctrl.GetTimeUntilWaveToggle():F1} sec";
#endif
    }

}
