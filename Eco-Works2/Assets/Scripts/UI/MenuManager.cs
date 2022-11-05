using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public float panelOpenLerpSpeed;

    public CanvasGroup playPanel;
    public CanvasGroup settingsPanel;
    public CanvasGroup creditsPanel;

    bool playOpen;
    bool settingsOpen;
    bool creditsOpen;

    public Toggle tutorialToggle;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this);
    }


    public AudioManager menuAudioManager;
    public LevelLoader levelLoader;

    public int mapSeed;
    public void PlayButtonOnClick()
    {
        menuAudioManager.fadeOut = true;
        levelLoader.LoadLevel(1, mapSeed);
    }

    public void QuitOnClick()
    {
        Application.Quit();
    }

    public void PlayPanelOnClick()
    {
        playOpen = !playOpen;
        settingsOpen = false;
        creditsOpen = false;
    }

    public void SettingsPanelOnClick()
    {
        settingsOpen = !settingsOpen;
        playOpen = false;
        creditsOpen = false;
    }

    public void CreditsPanelOnClick()
    {
        creditsOpen = !creditsOpen;
        settingsOpen = false;
        playOpen = false;
    }

    public void NewSeed(string newSeed)
    {
        mapSeed = int.Parse(newSeed);
    }


    [Header("Graphics")]
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        tutorialToggle.isOn = (PlayerPrefs.GetInt("Tutorial", 1) == 1);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetTutorial(bool doTutorial)
    {
        int value = 1;
        if (!doTutorial)
        {
            value = 0;
        }
        PlayerPrefs.SetInt("Tutorial", value);
    }

    private void Update()
    {

        if (creditsPanel == null) return;
        if (creditsOpen)
        {
            creditsPanel.alpha = Mathf.Lerp(creditsPanel.alpha, 1, panelOpenLerpSpeed * Time.deltaTime);
            creditsPanel.blocksRaycasts = true;
        }
        else
        {
            creditsPanel.alpha = Mathf.Lerp(creditsPanel.alpha, 0, panelOpenLerpSpeed * Time.deltaTime);
            creditsPanel.blocksRaycasts = false;

        }


        if (settingsOpen)
        {
            settingsPanel.alpha = Mathf.Lerp(settingsPanel.alpha, 1, panelOpenLerpSpeed * Time.deltaTime);
            settingsPanel.blocksRaycasts = true;

        }
        else
        {
            settingsPanel.alpha = Mathf.Lerp(settingsPanel.alpha, 0, panelOpenLerpSpeed * Time.deltaTime);
            settingsPanel.blocksRaycasts = false;
        }


        if (playOpen)
        {
            playPanel.alpha = Mathf.Lerp(playPanel.alpha, 1, panelOpenLerpSpeed * Time.deltaTime);
            playPanel.blocksRaycasts = true;

        }
        else
        {
            playPanel.alpha = Mathf.Lerp(playPanel.alpha, 0, panelOpenLerpSpeed * Time.deltaTime);
            playPanel.blocksRaycasts = false;
        }
    }
}
