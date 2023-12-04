using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject winMenu;

    public Button continueButton;
    public Button restartButton;
    public Button exitFromPause;

    public Button tryAgainButton;
    public Button exitFromDeath;

    public Button nextLevelButton;
    public Button replayButton;
    public Button exitFromWin;

    public Slider hp;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Game.state = Game.State.Play;
        
        continueButton.onClick.AddListener(ContinueButton);
        restartButton.onClick.AddListener(Restart);
        exitFromPause.onClick.AddListener(Exit);
        
        tryAgainButton.onClick.AddListener(Restart);
        exitFromDeath.onClick.AddListener(Exit);
        
        nextLevelButton.onClick.AddListener(NextLevel);
        replayButton.onClick.AddListener(Restart);
        exitFromWin.onClick.AddListener(Exit);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (Game.state)
            {
                case Game.State.Play:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    pauseMenu.SetActive(true);
                    Game.state = Game.State.Pause;
                    break;
                
                case Game.State.Pause:
                    ContinueButton();
                    break;
                
                default:
                    Exit();
                    break;
            }
        }
    }

    void Exit()
    {
        Game.state = Game.State.MainMenu;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void ContinueButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Game.state = Game.State.Play;
    }

    void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void NextLevel()
    {
        Game.level += 1;
        switch (Game.level)
        {
            case 1:
                SceneManager.LoadScene("Level1");
                break;
            case 2:
                SceneManager.LoadScene("Level2");
                break;
            case 3:
                SceneManager.LoadScene("Level3");
                break;
            case 4:
                SceneManager.LoadScene("Level4");
                break;
            case 5:
                SceneManager.LoadScene("Level5");
                break;
            
            default:
                Exit();
                break;
        }
    }
}
