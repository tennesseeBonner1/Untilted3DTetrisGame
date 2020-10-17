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
    private PlayerControls controls;    //The PlayerControls 

    private Vector2 rotate; //Only a 2D vector is needed since we only rotate left and right(for now)

    public GameObject cameraPrefab;
    public GameObject planePrefab;

    private new GameObject[] camera;

    public int cameraDFC = 5;

    private int cameraVal;

    private float cameraTurnTimer;       
    private float cameraTurnTime = 0.4f;

    private float cameraTiltTimer;
    private float cameraTiltTime = 0.3f;

    public float rotationShakeDuration = 1f;
    public float rotationShakeSpeed = 1f;

    public static int cameraOrientation;

    //Gets the controls
    void Awake()
    {
        //Setup the controls for the camera
        controls = new PlayerControls();
        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
        controls.Gameplay.SnapCam.performed += ctx => SnapCamera();

        //Create all the cameras
        CreateCameras();

        CreateBoard();
    }

    void CreateCameras()
    {
        //Get the dimensions of the board, divide those numbers, then take the minimum of the two
        int xDist = Setup.startBoard[0] / 2;
        int zDist = Setup.startBoard[2] / 2;

        int minDist = xDist;

        if (zDist < xDist)
            minDist = zDist;

        //Add the camera distance from center
        minDist += cameraDFC;

        //Calculate 1, 1/2, and sqrt(3)/2 for the UNIT circle
        float wholeDist = (float)minDist;
        float halfDist = wholeDist / 2f;
        float almstWhleDist = wholeDist * 0.86602540378f;

        //Create the cameras, correct their positions, then activate camera[0]
        camera = new GameObject[12];
        
        camera[0] = Instantiate(cameraPrefab, new Vector3(0, wholeDist, -wholeDist), Quaternion.Euler(45, 0, 0));
        camera[1] = Instantiate(cameraPrefab, new Vector3(halfDist, wholeDist, -almstWhleDist), Quaternion.Euler(45, -30, 0));
        camera[2] = Instantiate(cameraPrefab, new Vector3(almstWhleDist, wholeDist, -halfDist), Quaternion.Euler(45, -60, 0));
        camera[3] = Instantiate(cameraPrefab, new Vector3(wholeDist, wholeDist, 0), Quaternion.Euler(45, -90, 0));
        camera[4] = Instantiate(cameraPrefab, new Vector3(almstWhleDist, wholeDist, halfDist), Quaternion.Euler(45, -120, 0));
        camera[5] = Instantiate(cameraPrefab, new Vector3(halfDist, wholeDist, almstWhleDist), Quaternion.Euler(45, -150, 0));
        camera[6] = Instantiate(cameraPrefab, new Vector3(0, wholeDist, wholeDist), Quaternion.Euler(45, -180, 0));
        camera[7] = Instantiate(cameraPrefab, new Vector3(-halfDist, wholeDist, almstWhleDist), Quaternion.Euler(45, 150, 0));
        camera[8] = Instantiate(cameraPrefab, new Vector3(-almstWhleDist, wholeDist, halfDist), Quaternion.Euler(45, 120, 0));
        camera[9] = Instantiate(cameraPrefab, new Vector3(-wholeDist, wholeDist, 0), Quaternion.Euler(45, 90, 0));
        camera[10] = Instantiate(cameraPrefab, new Vector3(-almstWhleDist, wholeDist, -halfDist), Quaternion.Euler(45, 60, 0));
        camera[11] = Instantiate(cameraPrefab, new Vector3(-halfDist, wholeDist, -almstWhleDist), Quaternion.Euler(45, 30, 0));

        camera[0].GetComponent<Camera>().enabled = true;
        camera[1].GetComponent<Camera>().enabled = false;
        camera[2].GetComponent<Camera>().enabled = false;
        camera[3].GetComponent<Camera>().enabled = false;
        camera[4].GetComponent<Camera>().enabled = false;
        camera[5].GetComponent<Camera>().enabled = false;
        camera[6].GetComponent<Camera>().enabled = false;
        camera[7].GetComponent<Camera>().enabled = false;
        camera[8].GetComponent<Camera>().enabled = false;
        camera[9].GetComponent<Camera>().enabled = false;
        camera[10].GetComponent<Camera>().enabled = false;
        camera[11].GetComponent<Camera>().enabled = false;

        cameraVal = 0;
    }

    void CreateBoard()
    {
        int bsX = Setup.startBoard[0];
        int bsY = Setup.startBoard[1];
        int bsZ = Setup.startBoard[2];

        float startXPos = -Mathf.Round((bsX / (float)2.0));
        float startZPos = (float)(bsZ/2);

        startXPos += .5f;
        startZPos -= .5f;
        for (int i = 0; i < 2; i++)
        {
            for (int x = 0; x < bsX; x++)
                for (int z = 0; z < bsZ; z++)
                    Instantiate(planePrefab, new Vector3(startXPos + x, -0.5f, startZPos - z), Quaternion.identity);
            /*
            for (int x = 0; x < bsX; x++)
                for (int z = 0; z < bsZ; z++)
                    Instantiate(planePrefab, new Vector3(startXPos + x, (-bsY  - 0.5f), startZPos - z), Quaternion.identity);
            */
        }
    }
    //Checks if rotate is being modified
    void Update()
    {
        cameraTurnTimer -= Time.deltaTime;
        if (cameraTurnTimer <= 0)
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

        cameraTiltTimer -= Time.deltaTime;
        if (cameraTiltTimer <= 0)
        {
            if (rotate.y > 0.3 || rotate.y < -0.3)
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
            else
            {
                //Debug.Log("DROP");
            }
        }
    }
    void SnapCamera()
    {
        cameraVal = 0;
        ChangeCamera();
    }

    void TiltCamera(bool direction)
    {
        //For peaking up and down
    }

    void TurnCamera(bool direction)
    {
        if (direction)
        {
            cameraVal += 1;

            if (cameraVal > 11)
                cameraVal = 0;

            StartCoroutine(RightTurn());
        }
        else
        {
            cameraVal -= 1;

            if (cameraVal < 0)
                cameraVal = 11;

            StartCoroutine(LeftTurn());
        }
        ChangeCamera();
    }

    IEnumerator RightTurn()
    {
        Vector3[] camPos = new Vector3[12];

        for (int i = 0; i < 12; i++)
            camPos[i] = camera[i].transform.eulerAngles;

        float elapsed = 0.0f;

        while (elapsed < rotationShakeDuration)
        {
            for (int i = 0; i < 12; i++)
                camera[i].transform.Rotate(Vector3.up * rotationShakeSpeed, Space.World);

            elapsed += Time.deltaTime;

            yield return null;
        }

        elapsed = 0.0f;

        while (elapsed < (rotationShakeDuration*.5f))
        {
            for (int i = 0; i < 12; i++)
                camera[i].transform.Rotate(Vector3.down * rotationShakeSpeed, Space.World);

            elapsed += Time.deltaTime;

            yield return null;
        }

        for (int i = 0; i < 12; i++)
            camera[i].transform.eulerAngles = camPos[i];
    }

    IEnumerator LeftTurn()
    {
        Vector3[] camPos = new Vector3[12];

        for (int i = 0; i < 12; i++)
            camPos[i] = camera[i].transform.eulerAngles;

        float elapsed = 0.0f;

        while (elapsed < rotationShakeDuration)
        {
            for (int i = 0; i < 12; i++)
                camera[i].transform.Rotate(Vector3.down * rotationShakeSpeed, Space.World);

            elapsed += Time.deltaTime;

            yield return null;
        }

        while (elapsed < rotationShakeDuration * .5f)
        {
            for (int i = 0; i < 12; i++)
                camera[i].transform.Rotate(Vector3.up * rotationShakeSpeed, Space.World);

            elapsed += Time.deltaTime;

            yield return null;
        }

        for (int i = 0; i < 12; i++)
            camera[i].transform.eulerAngles = camPos[i];
    }


    //Change the active camera
    void ChangeCamera()
    {
        for (int i = 0; i < 12; i++)
            camera[i].GetComponent<Camera>().enabled = false;

        switch (cameraVal)
        {
            case (0):
                camera[0].GetComponent<Camera>().enabled = true;
                cameraOrientation = 0;
                break;
            case (1):
                camera[1].GetComponent<Camera>().enabled = true;
                cameraOrientation = 0;
                break;
            case (2):
                camera[2].GetComponent<Camera>().enabled = true;
                cameraOrientation = 1;
                break;
            case (3):
                camera[3].GetComponent<Camera>().enabled = true;
                cameraOrientation = 1;
                break;
            case (4):
                camera[4].GetComponent<Camera>().enabled = true;
                cameraOrientation = 1;
                break;
            case (5):
                camera[5].GetComponent<Camera>().enabled = true;
                cameraOrientation = 2;
                break;
            case (6):
                camera[6].GetComponent<Camera>().enabled = true;
                cameraOrientation = 2;
                break;
            case (7):
                camera[7].GetComponent<Camera>().enabled = true;
                cameraOrientation = 2;
                break;
            case (8):
                camera[8].GetComponent<Camera>().enabled = true;
                cameraOrientation = 3;
                break;
            case (9):
                camera[9].GetComponent<Camera>().enabled = true;
                cameraOrientation = 3;
                break;
            case (10):
                camera[10].GetComponent<Camera>().enabled = true;
                cameraOrientation = 3;
                break;
            case (11):
                camera[11].GetComponent<Camera>().enabled = true;
                cameraOrientation = 0;
                break;
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
