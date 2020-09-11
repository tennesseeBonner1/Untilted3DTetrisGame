using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static int tutorialNumber = 0;
    public GameObject Panel1;
    public GameObject Panel2;
    public GameObject Panel3;
    public GameObject Panel4;
    public GameObject Panel5;
    public GameObject Panel6;

    private bool selectionTutorialDone = false;
    private void Awake()
    {
        Panel1.SetActive(false);
        Panel2.SetActive(false);
        Panel3.SetActive(false);
        Panel4.SetActive(false);
        Panel5.SetActive(false);
        Panel6.SetActive(false);
    }
    private void Update()
    {
        switch (tutorialNumber)
        {
            case 0:
                Panel1.SetActive(false);
                Panel2.SetActive(false);
                Panel3.SetActive(false);
                Panel4.SetActive(false);
                Panel5.SetActive(false);
                Panel6.SetActive(false);
                break;
            case 1:
                Panel1.SetActive(true);
                break;
            case 2:
                Panel1.SetActive(false);
                Panel2.SetActive(true);
                Panel3.SetActive(true);
                break;
            case 3:
                Panel2.SetActive(false);
                Panel3.SetActive(false);
                Panel4.SetActive(true);
                break;
            case 4:
                Panel4.SetActive(false);
                Panel5.SetActive(true);
                break;
            case 5:
                Panel5.SetActive(false);
                Panel6.SetActive(true);
                break;
            default:
                Panel1.SetActive(false);
                Panel2.SetActive(false);
                Panel3.SetActive(false);
                Panel4.SetActive(false);
                Panel5.SetActive(false);
                Panel6.SetActive(false);
                break;
        }
    }

    public void SelectionTutorialDone()
    {
        if (!selectionTutorialDone)
        {
            selectionTutorialDone = true;
            tutorialNumber = 4;
        }
    }
}
