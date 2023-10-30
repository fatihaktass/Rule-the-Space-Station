using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    //Sensivity
    public Text sensivityText;
    public Slider sensivitySliders;
    float sensivitySpeed;

    //Music
    public Text musicVolumeText;
    public Slider musicVolumeSlider;
    public AudioSource musicVolume;


    private void Start()
    {
        if (PlayerPrefs.HasKey("Sensivity"))
        {
            sensivitySliders.value = PlayerPrefs.GetFloat("Sensivity");
        }
        else
        {
            sensivitySliders.value = 500f;
            ChangeSensivity();
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVol");
        }
        else
        {
            musicVolumeSlider.value = 0.5f;
            ChangeMusicVolume();
        }
    }

    private void Update()
    {
        ChangeSensivity();
        ChangeMusicVolume();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SensivitySettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    
    public void GoToMenu()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ChangeSensivity()
    {
        sensivitySpeed = sensivitySliders.value;
        PlayerPrefs.SetFloat("Sensivity", sensivitySpeed);
        sensivityText.text = (sensivitySpeed / 1000).ToString("F2");        
    }

    public void ChangeMusicVolume()
    {
        musicVolume.volume = musicVolumeSlider.value;
        PlayerPrefs.SetFloat("MusicVol", musicVolume.volume);
        musicVolumeText.text = (musicVolume.volume).ToString("F2");
    }
}
