//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 12, 2020
//
//PauseMenu.cs
//Controls the pause menu and all the buttons associated with it
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;//If the game is currently paused

    public GameObject pauseMenuUI;     //The canvas for the pause menu 
    public GameObject pauseFirstButton;//The first button selected in the pause menu

    private LevelLoader ll;//The level loader

    private PlayerControls controls;//The player controls

    //Get the level loader, controls and Resume
    private void Awake()
    {
        GameObject destroyerOfWorlds = GameObject.Find("LevelLoader");
        ll = destroyerOfWorlds.GetComponent<LevelLoader>();

        controls = new PlayerControls();
        controls.Gameplay.Pause.performed += ctx => Decide();
        Resume();
    }

    //Called if pause button is pushed
    void Decide()
    {
        if (GameIsPaused)
            Resume();

        else
            Pause();
    }

    //Closes the menu and resumes the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    //Opens the menu and pauses the game
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        GameIsPaused = true;
    }

    //Reset the current level 
    public void Reset()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Reseting game");
        ll.ResetScene();
    }

    //Give up and go to the main menu
    public void GiveUp()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Giving Up");
        ll.LoadMainMenu();
    }

    //Close the application
    public void EndGame()
    {
        Debug.Log("Ending game");
        Application.Quit();
    }

    //Required for the controls
    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
