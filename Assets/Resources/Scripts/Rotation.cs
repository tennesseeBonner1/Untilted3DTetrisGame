//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//August 30, 2020
//
//Roatation.cs
//Used to run a Rotation routine for the player to rotate the piece.
//This Class contains private variables, public variables, a Start, Update, private functions and a post rotation routine to run if undo is not pushed.
//Public values are controlled in GameMaster in order to use the rotation script as a calculator for all parts of piece rotation.
//GameMaster also will use the values determined during the rotation routine to use in the game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotation : MonoBehaviour
{
    public bool Rotating;
    public bool undo;

    private float rotationPressingTimer;
    private float rotationPressingTime = 0.12f;

    public bool[,,] RotatingMatrix { get; set; } = new bool[5,5,5];

    public GameObject RotatingObject;

    public Piece RotatingPiece;

    public AudioManager AudioMan;

    PlayerControls controls;

    Vector2 move;

    private bool tutorialFinished = false;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Confirm.performed += ctx => Confirm();
        controls.Gameplay.Undo.performed += ctx => Undo();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        Rotating = false;
        undo = false;
        RotatingObject = new GameObject();
        RotatingPiece = new Piece(0);

        GameObject am = GameObject.Find("AudioManager");
        AudioMan = am.GetComponent<AudioManager>();
    }

    //Activates when rotating is true and exits if either undo or confirm are pushed 
    void Update()
    {
        if (Rotating && !PauseMenu.GameIsPaused)
        {
            rotationPressingTimer -= Time.deltaTime;

            //If the timer hasn't been pushed for the specified rotationPressingTime the buttons are unlocked 
            if (rotationPressingTimer <= 0)
            {
                if (move.x > 0.1 || move.x < - 0.1)
                {
                    if (move.x > 0)
                    {
                        RotatingObject.transform.Rotate(Vector3.forward, -90f, Space.World);
                        rotationPressingTimer = rotationPressingTime;
                        Debug.Log("Right Pushed");
                        AudioMan.Play("Rotate");
                        for (int z = 0; z < 5; z++)
                        {
                            for (int x = 0; x < 5 / 2; x++)
                            {
                                for (int y = x; y < 5 - x - 1; y++)
                                {
                                    bool temp = RotatingMatrix[x, y, z];
                                    RotatingMatrix[x, y, z] = RotatingMatrix[y, 5 - 1 - x, z];
                                    RotatingMatrix[y, 5 - 1 - x, z] = RotatingMatrix[5 - 1 - x, 5 - 1 - y, z];
                                    RotatingMatrix[5 - 1 - x, 5 - 1 - y, z] = RotatingMatrix[5 - 1 - y, x, z];
                                    RotatingMatrix[5 - 1 - y, x, z] = temp;
                                }
                            }
                        }
                    }
                    else
                    {
                        RotatingObject.transform.Rotate(Vector3.forward, 90f, Space.World);
                        rotationPressingTimer = rotationPressingTime;
                        Debug.Log("Left Pushed");
                        AudioMan.Play("Rotate");
                        for (int i = 0; i < 3; i++)
                        {
                            for (int z = 0; z < 5; z++)
                            {
                                for (int x = 0; x < 5 / 2; x++)
                                {
                                    for (int y = x; y < 5 - x - 1; y++)
                                    {
                                        bool temp = RotatingMatrix[x, y, z];
                                        RotatingMatrix[x, y, z] = RotatingMatrix[y, 5 - 1 - x, z];
                                        RotatingMatrix[y, 5 - 1 - x, z] = RotatingMatrix[5 - 1 - x, 5 - 1 - y, z];
                                        RotatingMatrix[5 - 1 - x, 5 - 1 - y, z] = RotatingMatrix[5 - 1 - y, x, z];
                                        RotatingMatrix[5 - 1 - y, x, z] = temp;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (move.y > 0.1 || move.y < -0.1)
                {
                    if (move.y > 0)
                    {
                        RotatingObject.transform.Rotate(Vector3.left, -90f, Space.World);
                        rotationPressingTimer = rotationPressingTime;
                        Debug.Log("Up Pushed");
                        AudioMan.Play("Rotate");
                        for (int i = 0; i < 3; i++)
                        {
                            for (int x = 0; x < 5; x++)
                            {
                                for (int y = 0; y < 5 / 2; y++)
                                {
                                    for (int z = y; z < 5 - y - 1; z++)
                                    {
                                        bool temp = RotatingMatrix[x, y, z];

                                        RotatingMatrix[x, y, z] = RotatingMatrix[x, z, 5 - 1 - y];
                                        RotatingMatrix[x, z, 5 - 1 - y] = RotatingMatrix[x, 5 - 1 - y, 5 - 1 - z];
                                        RotatingMatrix[x, 5 - 1 - y, 5 - 1 - z] = RotatingMatrix[x, 5 - 1 - z, y];
                                        RotatingMatrix[x, 5 - 1 - z, y] = temp;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        RotatingObject.transform.Rotate(Vector3.left, 90f, Space.World);
                        rotationPressingTimer = rotationPressingTime;
                        Debug.Log("Down Pushed");
                        AudioMan.Play("Rotate");
                        for (int x = 0; x < 5; x++)
                        {
                            for (int y = 0; y < 5 / 2; y++)
                            {
                                for (int z = y; z < 5 - y - 1; z++)
                                {
                                    bool temp = RotatingMatrix[x, y, z];
                                    RotatingMatrix[x, y, z] = RotatingMatrix[x, z, 5 - 1 - y];
                                    RotatingMatrix[x, z, 5 - 1 - y] = RotatingMatrix[x, 5 - 1 - y, 5 - 1 - z];
                                    RotatingMatrix[x, 5 - 1 - y, 5 - 1 - z] = RotatingMatrix[x, 5 - 1 - z, y];
                                    RotatingMatrix[x, 5 - 1 - z, y] = temp;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void Confirm()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (Rotating)
            {
                if (!tutorialFinished)
                    Tutorial.tutorialNumber = 5;
                tutorialFinished = true;

                if (rotationPressingTimer <= 0)
                {
                    Debug.Log("Confirm pushed");
                    AudioMan.Play("Click");
                    RotatingMatrix = shrinkArray();
                    Rotating = false;
                }
            }
        }
    }

    private void Undo()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (Rotating)
            {
                if (rotationPressingTimer <= 0)
                {
                    undo = true;
                    Rotating = false;
                }
            }
        }
    }
    //Used once confirm is pushed to shrink the 5x5x5 array to a more condensed 4x4x4 representation of the piece(no need for a rotation axis anymore), with the piece on the "floor" or the matirx.
    private bool[,,] shrinkArray()
    {
        bool[,,] testArray = new bool[4, 4, 4];   
        bool[,,]  returnArray = new bool[4, 4, 4];
        int lowestLayer = 3;                      

        int[] checkOrder = { 2, 3, 1, 4, 0 };
        int[] elimVals = { 2, 2, 2 };
            
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (DimensionOccupied(i, checkOrder[j], RotatingMatrix) == true)
                    elimVals[i] = (int) checkOrder[j + 1];

                else
                    break;
            }
        }

        
        float diffX = (float)(2f - elimVals[0]);
        float diffY = (float)(2f - elimVals[1]);
        float diffZ = (float)(2f - elimVals[2]);
        
        Vector3 tempCenter = RotatingPiece.Center;
        
        if (diffX > 0)
        {
            tempCenter[0] = tempCenter[0] - .5f;
        }
        if (diffX < 0)
        {
            tempCenter[0] = tempCenter[0] + .5f;
        }
        if (diffY > 0)
        {
            tempCenter[1] = tempCenter[1] + .5f;
        }
        if (diffY < 0)
        {
            tempCenter[1] = tempCenter[1] - .5f;
        }
        if (diffZ > 0)
        {
            tempCenter[2] = tempCenter[2] - .5f;
        }
        if (diffZ < 0)
        {
            tempCenter[2] = tempCenter[2] + .5f;
        }
        

        int scanX = 0, scanY = 0, scanZ = 0;

        for (int a = 0; a < 4; a++)
        {
            scanY = 0;
            if (scanX == elimVals[0])
                ++scanX;
            
            for (int b = 0; b < 4; b++)
            {
                scanZ = 0;
                if (scanY == elimVals[1])
                    ++scanY;
                
                for (int c = 0; c < 4; c++)
                {
                    if (scanZ == elimVals[2])
                        ++scanZ;
               
                    testArray[a, b, c] = RotatingMatrix[scanX, scanY, scanZ];
                    ++scanZ;
                }
                ++scanY;
            }
            ++scanX;
        }
        
        for (int yLayer = 3; yLayer > -1; yLayer--)
        {
            if (DimensionOccupied(1, yLayer, testArray))
            {
                lowestLayer = yLayer;
                break;
            }
        }

        
        tempCenter[1] = tempCenter[1] - (float)(3 - lowestLayer);
        RotatingPiece.Center = tempCenter;

        int testPosition = lowestLayer;
        
        for (int i = 3; i > -1; i--)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (testPosition <= -1)
                        returnArray[j, i, k] = testArray[j, 3, k];
                    else
                        returnArray[j, i, k] = testArray[j, testPosition, k];
                }
            }
            testPosition--;
        }

        return returnArray;
    }

    //Essentially checks slices of the matricies at the specified value of the specified dimension
    private bool DimensionOccupied(int dimension, int value, bool [,,] check)
    {
        bool occupied = false;
        
        if (dimension == 0)
            for (int y = 0; y < System.Math.Pow(check.Length, (double)1 / 3); y++)
                for (int z = 0; z < System.Math.Pow(check.Length, (double)1 / 3); z++)
                    if (check[value, y, z] == true)
                        occupied = true;

        if (dimension == 1)
            for (int x = 0; x < System.Math.Pow(check.Length, (double)1 / 3); x++)
                for (int z = 0; z < System.Math.Pow(check.Length, (double)1 / 3); z++)
                    if (check[x, value, z] == true)
                        occupied = true;

        if (dimension == 2)
            for (int x = 0; x < System.Math.Pow(check.Length, (double)1 / 3); x++)
                for (int y = 0; y < System.Math.Pow(check.Length, (double)1 / 3); y++)
                    if (check[x, y, value] == true)
                        occupied = true;

        return occupied;
    }

    //After successful rotation the piece is refit to fit within the very center of a piece as big as the board that the piece will be dropped in 
    public void postRotation(int xDim, int zDim)
    {
        int xStart = calculatePiecePosition(xDim);  
        int zStart = calculatePiecePosition(zDim);  
        bool[,,] newPiece = new bool[xDim, 4, zDim];

        int offsetZ, offsetY, offsetX = 0;
        for (int x = xStart; x < (xStart + 4); x++)
        {
            offsetY = 0;
            for (int y = 0; y < 4; y++)
            {
                offsetZ = 0;
                for (int z = zStart; z < (zStart + 4); z++)
                {
                    newPiece[x,y,z] = RotatingMatrix[offsetX,offsetY,offsetZ];
                    offsetZ += 1;
                }
                offsetY += 1;
            }
            offsetX += 1;
        }
        RotatingMatrix = newPiece;
    }

    //Determines where the center of the board is 
    private int calculatePiecePosition(int number)
    {
        float x = (float)(number - 4);

        if (x == 0)
            return (int)x;
        else
            return (int)Mathf.Round((x / (float)2.0));
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
