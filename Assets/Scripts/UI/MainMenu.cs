using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject levelChoiceMenu;

    public Button startButton;
    public Button settingsButton;
    public Button quitButton;
    
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    public Button level5Button;
    public Button backFromLevelChoice;

    public Button musicOnButton;
    public Button musicOffButton;
    public Button soundsOnButton;
    public Button soundsOffButton;
    public Button backFromSettings;


    void Start()
    {
        startButton.onClick.AddListener(StartButton);
        settingsButton.onClick.AddListener(SettingsButton);
        quitButton.onClick.AddListener(QuitButton);
        
        level1Button.onClick.AddListener(Level1Button);
        level2Button.onClick.AddListener(Level2Button);
        level3Button.onClick.AddListener(Level3Button);
        level4Button.onClick.AddListener(Level4Button);
        level5Button.onClick.AddListener(Level5Button);
        backFromLevelChoice.onClick.AddListener(BackFromLevelChoice);
        
        musicOnButton.onClick.AddListener(MusicOnButton);
        musicOffButton.onClick.AddListener(MusicOffButton);
        soundsOnButton.onClick.AddListener(SoundsOnButton);
        soundsOffButton.onClick.AddListener(SoundsOffButton);
        backFromSettings.onClick.AddListener(BackFromSettings);

        if (Game.musicOn)
            MusicOffButton();
        else
            MusicOnButton();
        
        if (Game.soundsOn)
            SoundsOffButton();
        else
            SoundsOnButton();
    }

    void StartButton()
    {
        mainMenu.SetActive(false);
        levelChoiceMenu.SetActive(true);
    }
    
    void SettingsButton()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    
    void QuitButton()
    {
        Application.Quit();
    }
    
    void Level1Button()
    {
        Game.level = 1;
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    
    void Level2Button()
    {
        Game.level = 2;
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
    }
    
    void Level3Button()
    {
        Game.level = 3;
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
    }
    
    void Level4Button()
    {
        Game.level = 4;
        SceneManager.LoadScene("Level4", LoadSceneMode.Single);
    }
    
    void Level5Button()
    {
        Game.level = 5;
        SceneManager.LoadScene("Level5", LoadSceneMode.Single);
    }

    void BackFromLevelChoice()
    {
        levelChoiceMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void MusicOnButton()
    {
        if (Game.musicOn)
            MusicPlayer.source.Stop();
        
        Game.musicOn = false;
        musicOnButton.gameObject.SetActive(false);
        musicOffButton.gameObject.SetActive(true);
    }
    
    void MusicOffButton()
    {
        if (!Game.musicOn)
            MusicPlayer.source.Play();
        
        Game.musicOn = true;
        musicOnButton.gameObject.SetActive(true);
        musicOffButton.gameObject.SetActive(false);
    }
    
    void SoundsOnButton()
    {
        Game.soundsOn = false;
        soundsOnButton.gameObject.SetActive(false);
        soundsOffButton.gameObject.SetActive(true);
    }
    
    void SoundsOffButton()
    {
        Game.soundsOn = true;
        soundsOnButton.gameObject.SetActive(true);
        soundsOffButton.gameObject.SetActive(false);
    }
    
    void BackFromSettings()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
