//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 12, 2020
//
//MainMenu.cs
//Controls the main menu and all the buttons associated with that scene
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public float waitTime = 2f;
    public GameObject startScreen;
    public GameObject mM;  //Main menu canvas
    public GameObject opts;//Options Canvas
    public GameObject mL;  //Make Level Canvas

    private GameObject destroyerOfWorlds;//The level loader

    public GameObject sSButton;
    public GameObject mMFirstButton;  //The first selected button in the main menu 
    public GameObject optsFirstButton;//The first selected button in the options menu 
    public GameObject mLFirstButton;  //The first selected button in the make level menu 

    public static int[] startBoard = { 4, 4, 4 };
    public static int[] pieces = { 1, 0, 2, 5, 1, 2, 2, 3 };

    //Get the level loader and set the right menu active
    void Awake()
    {
        destroyerOfWorlds = new GameObject();
        destroyerOfWorlds = GameObject.Find("LevelLoader");

        mM.SetActive(false);
        opts.SetActive(false);
        mL.SetActive(false);
        startScreen.SetActive(false);

        StartCoroutine(Starting());
    }

    IEnumerator Starting()
    {
        yield return new WaitForSeconds(waitTime);

        startScreen.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(sSButton);
    }
    public void LoadMainMenu()
    {
        startScreen.SetActive(false);
        Back();
    }
    //Hide all the canvases and load the level
    public void LoadLevel()
    {
        mM.SetActive(false);
        opts.SetActive(false);
        mL.SetActive(false);

        for (int i = 0; i < 3; i++)
            Setup.startBoard[i] = startBoard[i];

        for (int i = 0; i < 8; i++)
            Setup.pieces[i] = pieces[i];

        LevelLoader ll = destroyerOfWorlds.GetComponent<LevelLoader>();
        ll.LoadNextLevel();
    }

    //Open up the make level menu
    public void MakeLevel()
    {
        mM.SetActive(false);
        opts.SetActive(false);
        mL.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mLFirstButton);
    }

    //Open up the options menu
    public void Options()
    {
        mM.SetActive(false);
        opts.SetActive(true);
        mL.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optsFirstButton);
    }

    //Return to main menu 
    public void Back()
    {
        mM.SetActive(true);
        opts.SetActive(false);
        mL.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mMFirstButton);
    }
}
