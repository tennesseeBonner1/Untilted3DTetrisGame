//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//August 30, 2020
//
//Basker.cs
//Used to run a basker routine for the player to "bask in the glory of their accomplishments".
//This Class contains public variables, a Start, and Update function 
//Public values are controlled in GameMaster to make basking true and reset undo
//Essentially once basking starts the player must either press confirm or undo to procede.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Basker : MonoBehaviour
{
    public bool basking;
    public bool undo;
    public bool undoable;

    PlayerControls controls;

    private bool tutorialFinished = false;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Confirm.performed += ctx => Confirm();
        controls.Gameplay.Undo.performed += ctx => Undo();
        basking = false;
        undo = false;
        undoable = false;
    }

    void Confirm()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (Tutorial.tutorialNumber > 1)
            {
                if (!tutorialFinished)
                    Tutorial.tutorialNumber = 3;
                tutorialFinished = true;
                Debug.Log("Going back");
                if (basking)
                    basking = false;
            }
        }
    }

    void Undo()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (basking)
                if (undoable)
                {
                    undo = true;
                    basking = false;
                }
        }
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
