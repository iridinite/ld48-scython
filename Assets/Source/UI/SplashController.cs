using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{
    [Header("Logotype")]
    public GameObject m_Logo;
    public AudioClip m_LogoAppear;

    [Header("Progression")]
    public string m_NextScene;

    private void Awake()
    {
        m_Logo.SetActive(false);
        StartCoroutine(SplashAnimationCoroutine());
    }

    private IEnumerator SplashAnimationCoroutine()
    {
        // Await at least a physics update, because initial boot eats up a few frames
        yield return new WaitForFixedUpdate();
        yield return new WaitForSecondsRealtime(0.15f);

        // Pop in
        AudioHelper.PlayOneshot2D(m_LogoAppear);
        m_Logo.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);

        // Pop out
        m_Logo.SetActive(false);
        yield return new WaitForSecondsRealtime(0.05f);

        // Move to next scene
        SceneManager.LoadScene(m_NextScene);
    }
}
