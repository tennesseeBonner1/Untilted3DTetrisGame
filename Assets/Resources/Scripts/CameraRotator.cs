using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float Speed;

    PlayerControls controls;

    Vector2 rotate;

    private bool tutorialFinished = false;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (rotate.x > 0.1 || rotate.x < -0.1)
        {
            if (!tutorialFinished)
                Tutorial.tutorialNumber = 2;
            tutorialFinished = true;
            if (rotate.x > 0)
            {
                transform.Rotate(0, -Speed * Time.deltaTime, 0);
            }
            else
            {
                transform.Rotate(0, Speed * Time.deltaTime, 0);
            }
        }

        else
        {
            transform.Rotate(0, 0, 0);

            if (!tutorialFinished)
                Tutorial.tutorialNumber = 1;
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
