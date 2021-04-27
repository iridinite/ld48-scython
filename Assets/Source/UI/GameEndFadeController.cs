using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndFadeController : MonoBehaviour
{

    public Image m_Fader;

    private void Awake()
    {
        m_Fader.gameObject.SetActive(false);
    }

    public void FadeToScene(string scene)
    {
        StartCoroutine(FadeCoroutine(scene));
    }

    private IEnumerator FadeCoroutine(string scene)
    {
        m_Fader.color = new Color(0f, 0f, 0f, 0f);
        m_Fader.gameObject.SetActive(true);

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.fixedDeltaTime;
            m_Fader.color = new Color(0f, 0f, 0f, alpha);
            yield return new WaitForFixedUpdate();
        }

        SceneManager.LoadScene(scene);
    }

}
