//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 11, 2020
//
//CameraRotator.cs
//Seperate control specifically to turn the board
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float Speed; //The speed at which the camera turns 

    private PlayerControls controls;    //The PlayerControls 

    private Vector2 rotate; //Only a 2D vector is needed since we only rotate left and right(for now)

    private bool tutorialFinished = false;  //If the camera turn tutorial is finished

    //Gets the controls
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
    }

    //Checks if rotate is being modified
    void Update()
    {
        if (rotate.x > 0.1 || rotate.x < -0.1)
        {
            if (!tutorialFinished)
                Tutorial.tutorialNumber = 2;
            tutorialFinished = true;
            if (rotate.x > 0)
                transform.Rotate(0, -Speed * Time.deltaTime, 0);

            else
                transform.Rotate(0, Speed * Time.deltaTime, 0);
        }

        //Stops the rotation so there isn't some weird drift where the board keeps turning
        else
        {
            transform.Rotate(0, 0, 0);

            if (!tutorialFinished)
                Tutorial.tutorialNumber = 1;
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
