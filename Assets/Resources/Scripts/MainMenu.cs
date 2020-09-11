using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject mM;
    public GameObject opts;
    public GameObject mL;
    private GameObject destroyerOfWorlds;

    public GameObject mMFirstButton;
    public GameObject optsFirstButton;
    public GameObject mLFirstButton;

    void Awake()
    {
        destroyerOfWorlds = new GameObject();
        destroyerOfWorlds = GameObject.Find("LevelLoader");

        mM.SetActive(true);
        opts.SetActive(false);
        mL.SetActive(false);
    }

    public void LoadLevel()
    {
        LevelLoader ll = destroyerOfWorlds.GetComponent<LevelLoader>();
        ll.LoadNextLevel();
    }

    public void MakeLevel()
    {
        mM.SetActive(false);
        opts.SetActive(false);
        mL.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mLFirstButton);


    }

    public void Options()
    {
        mM.SetActive(false);
        opts.SetActive(true);
        mL.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optsFirstButton);
    }

    public void Back()
    {
        mM.SetActive(true);
        opts.SetActive(false);
        mL.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mMFirstButton);
    }
}
