//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//August 30, 2020
//
//Translation.cs
//Used to run a Translation routine for the player to translate the piece.
//This Class contains private variables, public variables, a Start, Update, and a private function for confirming if a drop is valid.
//Public values are controlled in GameMaster in order to use the translation script as a calculator for all parts of piece translation.
//GameMaster also will use the values determined during the translation routine to use in the game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Translation : MonoBehaviour
{
    public bool Translating;
    public bool undo;

    private float translatingPressingTimer;
    private float translatingPressingTime = 0.12f;
                       
    public bool[,,] TranslatingMatrix { get; set; } = new bool[4,4,4];
    public bool[,,] BoardMatrix { get; set; } = new bool[4, 4, 4];

    public GameObject TranslatingObject;

    public AudioManager AudioMan;

    PlayerControls controls;

    Vector2 move;

    private bool tutorialFinished = false;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Confirm.performed += ctx => Confirm();
        controls.Gameplay.Undo.performed += ctx => Undo();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();

        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        Translating = false;
        undo = false;
        TranslatingObject = new GameObject();
        GameObject am = GameObject.Find("AudioManager");
        AudioMan = am.GetComponent<AudioManager>();
    }

    //Activates when translating is true and exits if either undo or confirm are pushed (if confirm results in a successful drop)
    void Update()
    {
        if (Translating && !PauseMenu.GameIsPaused)
        {
            translatingPressingTimer -= Time.deltaTime;

            int cpX = TranslatingMatrix.GetLength(0);
            int cpY = TranslatingMatrix.GetLength(1);
            int cpZ = TranslatingMatrix.GetLength(2);

            bool[,,] cleanMatrix = new bool[cpX, cpY, cpZ];

            if (translatingPressingTimer <= 0)
            {
                if (move.x > 0.1 || move.x < -0.1)
                {
                    if (move.x > 0)
                    {
                        Debug.Log("Right Pushed");
                        translatingPressingTimer = translatingPressingTime;

                        for (int y = 0; y < cpY; y++)
                            for (int z = 0; z < cpZ; z++)
                                if (TranslatingMatrix[(cpX - 1), y, z] == true)
                                    return;
                        for (int x = 1; x < cpX; x++)
                            for (int y = 0; y < cpY; y++)
                                for (int z = 0; z < cpZ; z++)
                                    cleanMatrix[x, y, z] = TranslatingMatrix[(x - 1), y, z];
                        TranslatingMatrix = cleanMatrix.Clone() as bool[,,];
                        TranslatingObject.transform.Translate(1, 0, 0, Space.World);
                        AudioMan.Play("Translate");
                    }
                    else
                    {
                        Debug.Log("Left Pushed");
                        translatingPressingTimer = translatingPressingTime;

                        for (int y = 0; y < cpY; y++)
                            for (int z = 0; z < cpZ; z++)
                                if (TranslatingMatrix[0, y, z] == true)
                                    return;

                        for (int x = 0; x < (cpX - 1); x++)
                            for (int y = 0; y < cpY; y++)
                                for (int z = 0; z < cpZ; z++)
                                    cleanMatrix[x, y, z] = TranslatingMatrix[(x + 1), y, z];
                        TranslatingMatrix = cleanMatrix.Clone() as bool[,,];
                        TranslatingObject.transform.Translate(-1, 0, 0, Space.World);
                        AudioMan.Play("Translate");
                    }
                }
                else if (move.y > 0.1 || move.y < -0.1)
                {
                    if (move.y > 0)
                    {
                        Debug.Log("Up Pushed");
                        translatingPressingTimer = translatingPressingTime;

                        for (int x = 0; x < cpX; x++)
                            for (int y = 0; y < cpY; y++)
                                if (TranslatingMatrix[x, y, (cpZ - 1)] == true)
                                    return;
                        for (int x = 0; x < cpX; x++)
                            for (int y = 0; y < cpY; y++)
                                for (int z = 1; z < cpZ; z++)
                                    cleanMatrix[x, y, z] = TranslatingMatrix[x, y, (z - 1)];
                        TranslatingMatrix = cleanMatrix.Clone() as bool[,,];
                        TranslatingObject.transform.Translate(0, 0, 1, Space.World);
                        AudioMan.Play("Translate");
                    }
                    else
                    {
                        Debug.Log("Down Pushed");
                        translatingPressingTimer = translatingPressingTime;

                        for (int x = 0; x < cpX; x++)
                            for (int y = 0; y < cpY; y++)
                                if (TranslatingMatrix[x, y, 0] == true)
                                    return;

                        for (int x = 0; x < cpX; x++)
                            for (int y = 0; y < cpY; y++)
                                for (int z = 0; z < (cpZ - 1); z++)
                                    cleanMatrix[x, y, z] = TranslatingMatrix[x, y, (z + 1)];
                        TranslatingMatrix = cleanMatrix.Clone() as bool[,,];
                        TranslatingObject.transform.Translate(0, 0, -1, Space.World);
                        AudioMan.Play("Translate");
                    }
                }
            }
        }
    }

    private void Confirm()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (Translating)
            {
                if (!tutorialFinished)
                    Tutorial.tutorialNumber = 6;
                tutorialFinished = true;

                if (translatingPressingTimer <= 0)
                {
                    translatingPressingTimer = translatingPressingTime;
                    Debug.Log("Confirm?");
                    bool result = AttemptDrop();

                    if (result)
                    {
                        AudioMan.Play("Click");
                        Translating = false;
                    }
                }
            }
        }
    }

    private void Undo()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (Translating)
            {
                if (translatingPressingTimer <= 0)
                {
                    translatingPressingTimer = translatingPressingTime;
                    undo = true;
                    Translating = false;
                }
            }
        }
    }
    
    //Checks from the bottom layer up to see if there is a valid place to drop (returns yes/no answer). Also it's important to note that the piece is dropped to the lowest level first.
    private bool AttemptDrop()
    {
        int cpX = TranslatingMatrix.GetLength(0);
        int cpY = TranslatingMatrix.GetLength(1);
        int cpZ = TranslatingMatrix.GetLength(2);

        int[,] pieceList = {{0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}};

        int pn = 0;

        for (int y = (cpY -1); y > -1; y--)
        {
            for (int x = 0; x < cpX; x++)
            {
                for(int z = 0; z < cpZ; z++)
                {
                    if(TranslatingMatrix[x,y,z] == true)
                    {
                        pieceList[pn, 0] = x;
                        pieceList[pn, 1] = y;
                        pieceList[pn, 2] = z;
                        pn += 1;
                    }
                }
            }
        }

        int cbX = BoardMatrix.GetLength(0);
        int cbY = BoardMatrix.GetLength(1);
        int cbZ = BoardMatrix.GetLength(2);

        int difference = cbY - 4;

        int[,] tempPieceList = {{ 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }};

        for (int i = 0; i < 4; i++)
        {
            tempPieceList[i, 0] = pieceList[i, 0] + difference;
            tempPieceList[i, 1] = pieceList[i, 1] + difference;
            tempPieceList[i, 2] = pieceList[i, 2] + difference;
        }

        int temp = 0;
        bool works = true;

        for (int y = (cbY - 1); y > -1; y--)
        {
            for (int i = 0; i < 4; i++)
            {
                temp = tempPieceList[i, 1];

                if (y < (cbY - 1))
                    tempPieceList[i, 1] = (temp - 1);
                if (tempPieceList[i, 1] < 0)
                    return false;
            }

            pieceList = tempPieceList;

            works = true;
            for(int i = 0; i < 4; i++)
            {
                if (BoardMatrix[tempPieceList[i,0], tempPieceList[i, 1], tempPieceList[i, 2]] == true)
                {
                    works = false;
                    break;
                }
            }

            if(works)
            {
                for (int i = 0; i < 4; i++)
                    BoardMatrix[tempPieceList[i, 0], tempPieceList[i, 1], tempPieceList[i, 2]] = true;
                
                Vector3 distance = new Vector3(0, -(y + 1), 0);
                TranslatingObject.transform.Translate(distance, Space.World);
                return true;
            }
        }
        return false;
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
