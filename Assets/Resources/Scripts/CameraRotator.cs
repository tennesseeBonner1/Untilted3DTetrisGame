//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 11, 2020
//
//CameraRotator.cs
//Seperate control specifically to turn the board
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private PlayerControls controls;    //The PlayerControls 

    private Vector2 rotate; //Only a 2D vector is needed since we only rotate left and right(for now)

    public GameObject cameraPrefab;
    public GameObject rotationCameraPrefab;
    public GameObject planePrefab;
    public GameObject BoardHolder;

    private GameObject[] camera;
    private GameObject[] rotationCamera;
    private GameObject CameraHolder;

    public int cameraDFC = 5;

    private int cameraVal;

    private float cameraTurnTimer;       
    private float cameraTurnTime = 0.4f;

    private float cameraTiltTimer;
    private float cameraTiltTime = 0.3f;

    public float tiltDuration = .5f;
    public float tiltSpeed = .2f;

    public static int cameraOrientation;

    private bool tilting = false;

    public float tiltTrigger = 0.3f;


    //Gets the controls
    void Awake()
    {
        //Setup the controls for the camera
        controls = new PlayerControls();
        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
        controls.Gameplay.SnapCam.performed += ctx => SnapCamera();

        CameraHolder = new GameObject();
        CameraHolder.name = "Cameras";

        BoardHolder = new GameObject();
        BoardHolder.name = "Board";

        //Create all the cameras
        CreateCameras();

        CreateBoard();
    }

    void CreateCameras()
    {
        //Get the dimensions of the board, divide those numbers, then take the minimum of the two
        int xDist = Setup.startBoard[0];
        int yDist = Setup.startBoard[1];
        int zDist = Setup.startBoard[2];

        int maxDist = xDist;

        if (zDist > xDist && zDist > yDist)
            maxDist = zDist;

        if (yDist > xDist && yDist > zDist)
            maxDist = yDist;

        cameraDFC *= maxDist;

        float rtTwOvTwo = (Mathf.Sqrt(2f)) / 2f;
        float largeEdge = (float)cameraDFC;
        float smallEdge = rtTwOvTwo * largeEdge;

        //Create the cameras, correct their positions, then activate camera[0]
        camera = new GameObject[4];
        
        camera[0] = Instantiate(cameraPrefab, new Vector3(smallEdge, 3 * maxDist, -smallEdge), Quaternion.Euler(35.264f, -45, 0)) as GameObject;
        camera[1] = Instantiate(cameraPrefab, new Vector3(smallEdge, 3 * maxDist, smallEdge), Quaternion.Euler(35.264f, -135, 0)) as GameObject;
        camera[2] = Instantiate(cameraPrefab, new Vector3(-smallEdge, 3 * maxDist, smallEdge), Quaternion.Euler(35.264f, 135, 0)) as GameObject;
        camera[3] = Instantiate(cameraPrefab, new Vector3(-smallEdge, 3 * maxDist, -smallEdge), Quaternion.Euler(35.264f, 45, 0)) as GameObject;

        camera[0].GetComponent<Camera>().enabled = true;
        camera[1].GetComponent<Camera>().enabled = false;
        camera[2].GetComponent<Camera>().enabled = false;
        camera[3].GetComponent<Camera>().enabled = false;

        camera[0].GetComponent<Camera>().orthographicSize = maxDist + 2;
        camera[1].GetComponent<Camera>().orthographicSize = maxDist + 2;
        camera[2].GetComponent<Camera>().orthographicSize = maxDist + 2;
        camera[3].GetComponent<Camera>().orthographicSize = maxDist + 2;

        for (int i = 0; i < 4; i++)
        {
            camera[i].name = "Camera " + i;
            camera[i].transform.SetParent(CameraHolder.transform);
        }
        //Adjust center position for odd values 
        if (xDist % 2 != 0)
        {
            if (xDist % 4 == 1)
                CameraHolder.transform.Translate(.5f, 0, 0);
            else
                CameraHolder.transform.Translate(-.5f, 0, 0);
        }
        if (zDist % 2 != 0)
        {
            if (zDist % 4 == 1)
                CameraHolder.transform.Translate(0, 0, .5f);
            
            else
                CameraHolder.transform.Translate(0, 0, -.5f);
        }

        rotationCamera = new GameObject[4];

        rotationCamera[0] = Instantiate(rotationCameraPrefab, new Vector3(rtTwOvTwo * 5, 5, -rtTwOvTwo * 5), Quaternion.Euler(35.264f, -45, 0)) as GameObject;
        rotationCamera[1] = Instantiate(rotationCameraPrefab, new Vector3(rtTwOvTwo * 5, 5, rtTwOvTwo * 5), Quaternion.Euler(35.264f, -135, 0)) as GameObject;
        rotationCamera[2] = Instantiate(rotationCameraPrefab, new Vector3(-rtTwOvTwo * 5, 5, rtTwOvTwo * 5), Quaternion.Euler(35.264f, 135, 0)) as GameObject;
        rotationCamera[3] = Instantiate(rotationCameraPrefab, new Vector3(-rtTwOvTwo * 5, 5, -rtTwOvTwo * 5), Quaternion.Euler(35.264f, 45, 0)) as GameObject;

        rotationCamera[0].GetComponent<Camera>().enabled = true;
        rotationCamera[1].GetComponent<Camera>().enabled = false;
        rotationCamera[2].GetComponent<Camera>().enabled = false;
        rotationCamera[3].GetComponent<Camera>().enabled = false;

        for (int i = 0; i < 4; i++)
        {
            rotationCamera[i].name = "RotationCamera " + i;
            rotationCamera[i].transform.SetParent(CameraHolder.transform);
        }

        cameraVal = 0;
    }

    void CreateBoard()
    {
        int bsX = Setup.startBoard[0];
        int bsY = Setup.startBoard[1];
        int bsZ = Setup.startBoard[2];

        float startXPos = -Mathf.Round((bsX / (float)2.0));
        float startZPos = -Mathf.Round((bsZ / (float)2.0));

        startXPos += .5f;
        startZPos += .5f;

        for (int x = 0; x < bsX; x++)
            for (int z = 0; z < bsZ; z++)
                Instantiate(planePrefab, new Vector3(startXPos + x, -0.5f, startZPos + z), Quaternion.identity).transform.SetParent(BoardHolder.transform);
                
        for (int x = 0; x < bsX; x++)
            for (int z = 0; z < bsZ; z++)
                Instantiate(planePrefab, new Vector3(startXPos + x, (-(bsY + 0.5f)), startZPos + z), Quaternion.identity).transform.SetParent(BoardHolder.transform);

    }
    //Checks if rotate is being modified
    void Update()
    {
        cameraTurnTimer -= Time.deltaTime;
        if (cameraTurnTimer <= 0 && !Selection.selecting)
        {
            if (rotate.x > 0.3 || rotate.x < -0.3)
            {
                if (rotate.x > 0)
                {
                    cameraTurnTimer = cameraTurnTime;
                    TurnCamera(true);
                }

                else
                {
                    cameraTurnTimer = cameraTurnTime;
                    TurnCamera(false);
                }
            }
        }


        if (rotate.y > tiltTrigger || rotate.y < -tiltTrigger)
        {
            cameraTiltTimer -= Time.deltaTime;
            if (cameraTiltTimer <= 0)
            {
                if (rotate.y > 0)
                {
                    cameraTiltTimer = cameraTiltTime;
                    TiltCamera(true);
                    //Debug.Log("PushU");
                }
                else
                {
                    cameraTiltTimer = cameraTiltTime;
                    TiltCamera(false);
                    //Debug.Log("PushD");
                }
            }
        }
        else
        {
            if (tilting)
            {
                StopCoroutine(UpTurn());
                StopCoroutine(DownTurn());

                CameraHolder.transform.eulerAngles = new Vector3(0, 0, 0);

                tilting = false;

            }
        }
        
    }
    void SnapCamera()
    {
        if (!tilting)
        {
            cameraVal = 0;
            ChangeCamera();
        }
    }

    void TiltCamera(bool direction)
    {
        if (!tilting)
        {
            tilting = true;
            //For peaking up and down
            if (direction)
            {
                StartCoroutine(UpTurn());
            }
            else
            {
                StartCoroutine(DownTurn());
            }
        }
    }

    void TurnCamera(bool direction)
    {
        if (!tilting)
        {
            if (direction)
            {
                cameraVal += 1;

                if (cameraVal > 3)
                    cameraVal = 0;
            }
            else
            {
                cameraVal -= 1;

                if (cameraVal < 0)
                    cameraVal = 3;
            }
            ChangeCamera();
        }
    }

    IEnumerator UpTurn()
    {
        float elapsed = 0.0f;

        while (elapsed < tiltDuration)
        {
            if (tilting)
                switch (cameraVal)
                {
                    case (0):
                        CameraHolder.transform.Rotate(Vector3.right * tiltSpeed, Space.Self);
                        CameraHolder.transform.Rotate(Vector3.forward * tiltSpeed, Space.Self);
                        break;
                    case (1):
                        CameraHolder.transform.Rotate(Vector3.left * tiltSpeed, Space.Self);
                        CameraHolder.transform.Rotate(Vector3.forward * tiltSpeed, Space.Self);
                        break;
                    case (2):
                        CameraHolder.transform.Rotate(Vector3.left * tiltSpeed, Space.Self);
                        CameraHolder.transform.Rotate(Vector3.back * tiltSpeed, Space.Self);
                        break;
                    case (3):
                        CameraHolder.transform.Rotate(Vector3.right * tiltSpeed, Space.Self);
                        CameraHolder.transform.Rotate(Vector3.back * tiltSpeed, Space.Self);
                        break;
                }

            elapsed += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator DownTurn()
    {
        float elapsed = 0.0f;

        while (elapsed < tiltDuration)
        {
            if(tilting)
                switch (cameraVal)
                {
                    case (0):
                        CameraHolder.transform.Rotate(Vector3.left * tiltSpeed, Space.Self);
                        CameraHolder.transform.Rotate(Vector3.back * tiltSpeed, Space.Self);
                        break;
                    case (1):
                        CameraHolder.transform.Rotate(Vector3.right * tiltSpeed, Space.Self);
                        CameraHolder.transform.Rotate(Vector3.back * tiltSpeed, Space.Self);
                        break;
                    case (2):
                        CameraHolder.transform.Rotate(Vector3.right * tiltSpeed, Space.Self);
                        CameraHolder.transform.Rotate(Vector3.forward * tiltSpeed, Space.Self);
                        break;
                    case (3):
                        CameraHolder.transform.Rotate(Vector3.left * tiltSpeed, Space.Self);
                        CameraHolder.transform.Rotate(Vector3.forward * tiltSpeed, Space.Self);
                        break;
                
                }

            elapsed += Time.deltaTime;

            yield return null;
        }
    }


    //Change the active camera
    void ChangeCamera()
    {
        if (!tilting)
        {
            for (int i = 0; i < 4; i++)
            {
                camera[i].GetComponent<Camera>().enabled = false;
                rotationCamera[i].GetComponent<Camera>().enabled = false;
            }

            switch (cameraVal)
            {
                case (0):
                    camera[0].GetComponent<Camera>().enabled = true;
                    rotationCamera[0].GetComponent<Camera>().enabled = true;
                    cameraOrientation = 0;
                    break;
                case (1):
                    camera[1].GetComponent<Camera>().enabled = true;
                    rotationCamera[1].GetComponent<Camera>().enabled = true;
                    cameraOrientation = 1;
                    break;
                case (2):
                    camera[2].GetComponent<Camera>().enabled = true;
                    rotationCamera[2].GetComponent<Camera>().enabled = true;
                    cameraOrientation = 2;
                    break;
                case (3):
                    camera[3].GetComponent<Camera>().enabled = true;
                    rotationCamera[3].GetComponent<Camera>().enabled = true;
                    cameraOrientation = 3;
                    break;
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
