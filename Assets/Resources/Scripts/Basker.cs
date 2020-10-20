//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 11, 2020
//
//Basker.cs
//Used to run a basker routine for the player to "bask in the glory of their accomplishments".
//Public values are controlled in GameMaster
//Essentially once basking starts the player must either press confirm or undo to procede.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Basker : MonoBehaviour
{
    public bool basking;    //If the basking state is active 
    public bool undo;       //If undo is pushed
    public bool undoable;   //If basking is undoable(not turn 0)

    private PlayerControls controls; //The PlayerControls 

    //Get the controls and set the public bools
    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Confirm.performed += ctx => Confirm();
        controls.Gameplay.Undo.performed += ctx => Undo();
        basking = false;
        undo = false;
        undoable = false;
    }

    //If confirm is pushed
    void Confirm()
    {
        //Only works if Game is not paused
        if (!PauseMenu.GameIsPaused)
            if (basking)
                basking = false;
    }

    //If Undo is pushed
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

    //These are required for the controls
    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
