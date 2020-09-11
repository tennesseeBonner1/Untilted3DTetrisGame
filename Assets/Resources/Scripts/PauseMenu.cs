using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    private GameObject destroyerOfWorlds;

    private LevelLoader ll;

    public GameObject pauseFirstButton;

    PlayerControls controls;

    private void Awake()
    {
        destroyerOfWorlds = GameObject.Find("LevelLoader");
        ll = destroyerOfWorlds.GetComponent<LevelLoader>();

        controls = new PlayerControls();
        controls.Gameplay.Pause.performed += ctx => Decide();
        Resume();
    }
    void Decide()
    {
        if (GameIsPaused)
        {
            Resume();
        }

        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        GameIsPaused = true;
    }

    public void Reset()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Reseting game");
        ll.ResetScene();
    }

    public void GiveUp()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Giving Up");
        ll.LoadMainMenu();
    }

    public void EndGame()
    {
        Debug.Log("Ending game");
        Application.Quit();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
