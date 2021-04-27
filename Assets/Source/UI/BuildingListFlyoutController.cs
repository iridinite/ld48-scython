using UnityEngine;

public class BuildingListFlyoutController : MonoBehaviour
{

    public Animator m_Target;

    public void TogglePanel()
    {
        m_Target.SetBool("Shown", !m_Target.GetBool("Shown"));
    }

    public void HidePanel()
    {
        m_Target.SetBool("Shown", false);
    }

}
