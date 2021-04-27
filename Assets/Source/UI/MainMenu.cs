using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{

    [Header("Main Menu")] 
    public GameObject m_MenuMain;
    public TextMeshProUGUI m_LabelVersion;

    [Header("Play Menu")]
    public GameObject m_MenuPlay;
    public GameObject m_MapPreviewPanel;
    public MapInfoAsset[] m_Maps;
    public TMP_Text m_MapTitle;
    public TMP_Text m_MapDesc;
    public RawImage m_MapPreviewImage;

    [Header("Settings - Video")] 
    public GameObject m_MenuSettings;
    public Toggle m_FullScreenCheck;
    public GameObject[] m_DisableInWebGL;
    public Toggle[] m_ResolutionToggles;

    [Header("Settings - Audio")]
    public AudioMixer m_AudioMixer;
    public Slider m_SliderVolumeMaster;
    public Slider m_SliderVolumeMusic;
    public TextMeshProUGUI m_LabelVolumeMaster;
    public TextMeshProUGUI m_LabelVolumeMusic;

    private static float m_GlobalVolumeMaster = 1f;
    private static float m_GlobalVolumeMusic = 1f;

    private void Start()
    {
        ToMainMenu();

        if (m_LabelVersion != null)
            m_LabelVersion.text = Application.version;

#if UNITY_WEBGL
        // Hide video settings in web because they don't make sense
        foreach (var obj in m_DisableInWebGL)
            obj.SetActive(false);
#endif

        InitializeView();
        UpdateVolumeLabels();
    }

    public void ShowLevel(int index)
    {
        MapInfoAsset info = m_Maps[index];
        m_MapTitle.text = info.m_ShortName;
        m_MapDesc.text = info.m_Description;
        m_MapPreviewImage.texture = info.m_Preview;

        m_MapPreviewPanel.SetActive(true);
    }

    public void HideLevel()
    {
        m_MapPreviewPanel.SetActive(false);
    }

    public void PlayLevel(int index)
    {
        // Activate the selected level
        MapInfoAsset info = m_Maps[index];
        MapList.ActiveMap = MapList.m_MapList[info.m_MapIndex];

        // Reset game variables
        GameSession.Instance = new GameSession();

        // Start the game
        SceneManager.LoadScene("L_Game");
    }

    public void ToMainMenu()
    {
        m_MenuMain.SetActive(true);
        m_MenuPlay.SetActive(false);
        m_MenuSettings.SetActive(false);
    }

    public void ToSettings()
    {
        m_MenuMain.SetActive(false);
        m_MenuPlay.SetActive(false);
        m_MenuSettings.SetActive(true);
    }

    public void ToMapSelect()
    {
        m_MenuMain.SetActive(false);
        m_MenuPlay.SetActive(true);
        m_MenuSettings.SetActive(false);
        m_MapPreviewPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void InitializeView()
    {
#if !UNITY_WEBGL
        m_FullScreenCheck.SetIsOnWithoutNotify(Screen.fullScreen);

        float resolutionRatio = Screen.height / 1080f;
        int resolutionIndex = 0;

        if (resolutionRatio < 0.99f)
            resolutionIndex = 0;
        else if (resolutionRatio > 0.99f && resolutionRatio < 1.01f)
            resolutionIndex = 1;
        else if (resolutionRatio > 1.01 && resolutionRatio < 1.99f)
            resolutionIndex = 2;
        else if (resolutionRatio > 1.99f)
            resolutionIndex = 3;

        m_ResolutionToggles[0].SetIsOnWithoutNotify(resolutionIndex == 0);
        m_ResolutionToggles[1].SetIsOnWithoutNotify(resolutionIndex == 1);
        m_ResolutionToggles[2].SetIsOnWithoutNotify(resolutionIndex == 2);
        m_ResolutionToggles[3].SetIsOnWithoutNotify(resolutionIndex == 3);
#endif

        m_SliderVolumeMaster.SetValueWithoutNotify(m_GlobalVolumeMaster);
        m_SliderVolumeMusic.SetValueWithoutNotify(m_GlobalVolumeMusic);
    }

    private void UpdateVolumeLabels()
    {
        m_GlobalVolumeMaster = m_SliderVolumeMaster.value;
        m_GlobalVolumeMusic = m_SliderVolumeMusic.value;

        m_LabelVolumeMaster.text = m_SliderVolumeMaster.value.ToString("P0");
        m_LabelVolumeMusic.text = m_SliderVolumeMusic.value.ToString("P0");
    }

    public void UpdateFullscreen()
    {
#if !UNITY_WEBGL
        Debug.Log("Fullscreen: " + m_FullScreenCheck.isOn);
        Screen.fullScreen = m_FullScreenCheck.isOn;
#endif
    }

    public void SelectResolution(int id)
    {
#if !UNITY_WEBGL
        switch (id)
        {
            case 0:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            default:
            case 1:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                break;
        }

        m_ResolutionToggles[0].SetIsOnWithoutNotify(id == 0);
        m_ResolutionToggles[1].SetIsOnWithoutNotify(id == 1);
        m_ResolutionToggles[2].SetIsOnWithoutNotify(id == 2);
        m_ResolutionToggles[3].SetIsOnWithoutNotify(id == 3);
#endif
    }

    public void ApplyVolumeSettings()
    {
        UpdateVolumeLabels();
        m_AudioMixer.SetFloat("VolumeMaster", Mathf.Log10(m_SliderVolumeMaster.value) * 20f);
        m_AudioMixer.SetFloat("VolumeMusic", Mathf.Log10(m_SliderVolumeMusic.value) * 20f);
    }

}