using UnityEngine;
using UnityEngine.SceneManagement;
using Time = UnityEngine.Time;

public class PauseMenu : MonoBehaviour
{

    public GameObject m_PauseMenu;
    public GameObject m_ExitButton;

    private void Awake()
    {
        ShowMenu(false);

#if UNITY_WEBGL
        // Cannot exit in WebGL
        m_ExitButton.SetActive(false);
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowMenu(!m_PauseMenu.activeSelf);
    }

    private void ShowMenu(bool show)
    {
        if (m_PauseMenu.activeSelf == show)
            return;

        m_PauseMenu.SetActive(show);
        Time.timeScale = show ? 0f : 1f;
    }

    public void Resume()
    {
        ShowMenu(false);
    }

    public void ToMenu(int sceneIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
#if !UNITY_WEBGL
        Application.Quit();
#endif
    }

}
