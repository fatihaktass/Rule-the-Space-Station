using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int killCount;
    public Text killCountText;
    public Text winKillCountText;

    public GameObject pauseMenuUI;
    public GameObject winLevelUI;
    public GameObject settingsUI;

    bool gameIsPaused = false;
    bool settingsOpen = false;

    //MusicVolume
    public Slider musicVolSlider;
    public Text musicVolText;
    public AudioSource musicAudioSource;

    //FXVolume
    public Slider FXVolumeSlider;
    public Text FXVolumeText;
    public AudioSource[] FXAudioSource;

    public static bool mainLevel = true;
    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            musicVolSlider.value = PlayerPrefs.GetFloat("MusicVol"); 
        }
        if (PlayerPrefs.HasKey("FXSound"))
        {
            FXVolumeSlider.value = PlayerPrefs.GetFloat("FXSound");
        }
        else
        {
            FXVolumeSlider.value = 0.5f;
            ChangeFXVolume();
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;   
        }
        if (gameIsPaused == true)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
            SettingsClose();
        }
        if (settingsOpen == true)
        {
            ChangeFXVolume();
            ChangeMusicVolume();
        }
    }

    public void ChangeStatic()
    {
        mainLevel = false;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        gameIsPaused = false;
    }



    public void RestartGame()
    {
       // FindObjectOfType<PlayerController>().checkPoint.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SettingsOpen()
    {
        settingsUI.SetActive(true);
        settingsOpen = true;
    }

    public void SettingsClose()
    {
        settingsUI.SetActive(false);
        settingsOpen = false;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level2");
    }
 
    public void ExitGame()
    {
        Application.Quit();
    }

    public void AddKill()
    {
        killCount++;
        killCountText.text = killCount.ToString();
        if (mainLevel == true)
        {
            winKillCountText.text = "Öldürülen düþman sayýsý: " + killCount.ToString();
        }
        
    }
    
    public void WinLevel()
    {
        winLevelUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 0;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ChangeMusicVolume()
    {
        musicAudioSource.volume = musicVolSlider.value;
        PlayerPrefs.SetFloat("MusicVol", musicAudioSource.volume);
        musicVolText.text = (musicAudioSource.volume).ToString("F2");
    }

    public void ChangeFXVolume()
    {
        FXAudioSource[0].volume = FXVolumeSlider.value; // Player
        FXAudioSource[1].volume = FXAudioSource[0].volume; // Gun
        FXAudioSource[2].volume = FXAudioSource[0].volume + 0.1f; // Enemy
        FXAudioSource[3].volume = FXAudioSource[0].volume; // Sword
        FXAudioSource[4].volume = FXAudioSource[0].volume; // Attack
        FXAudioSource[5].volume = FXAudioSource[0].volume; // Kick
        FXAudioSource[6].volume = FXAudioSource[0].volume; // Growl
        FXAudioSource[7].volume = FXAudioSource[0].volume; // DoorOpening
        PlayerPrefs.SetFloat("FXSound", FXAudioSource[0].volume);
        FXVolumeText.text = (FXAudioSource[0].volume).ToString("F2");

    }

}
