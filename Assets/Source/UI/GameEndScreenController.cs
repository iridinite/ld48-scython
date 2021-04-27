using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndScreenController : MonoBehaviour
{

    public GameObject m_ExitButton;

    private void Awake()
    {
        // just in case
        Time.timeScale = 1f;

#if UNITY_WEBGL
        m_ExitButton.SetActive(false);
#endif
    }

    public void OnReplayLevel()
    {
        // Reset game variables
        GameSession.Instance = new GameSession();

        // Relaunch the scene with the same MapList.ActiveMap
        SceneManager.LoadScene("L_Game");
    }

    public void OnExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExitToDesktop()
    {
#if !UNITY_WEBGL
        Application.Quit();
#endif
    }

}
