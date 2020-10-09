//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 11, 2020
//
//GameMaster.cs
//The GameMaster controls everything in the game scene, including: 
//- creating piece and board objects
//- runs the Basker, Selection, Rotation and Translation scripts 
//- uses Rotation and Translation scripts to calculate player input changes for pieces
using System.Collections;
using UnityEngine;

//Enumerator for the state the game is in
public enum GameState { SELECTION, ROTATION, TRANSLATION, BASKING }

public class GameMaster : MonoBehaviour
{
    private bool active;

    private GameState state;

    private Piece[][] Pieces;

    private Board[] Boards;

    private Piece CurrentPiece;

    private GameObject basker;
    private GameObject selector;
    private GameObject rotator;
    private GameObject translator;

    private int currentPiece;
    private int[] boardDimensions;

    private int currentTurn = 0;
    private int boardCount;

    public Transform RotationPosition;

    private GameObject[] pcs;

    public AudioManager AudioMan;


    //Create and initialize the piece and board objects and get all the gameobjects for the routines then start the state at basking
    private void Start()
    {
        //Get the audio manager
        GameObject am = GameObject.Find("AudioManager");
        AudioMan = am.GetComponent<AudioManager>();

        //Get the Setup
        boardDimensions = Setup.startBoard;
        int [] pieceCount =  Setup.pieces;
        boardCount =  Setup.getCount();

        Pieces = new Piece[pieceCount.Length][];
        Pieces[0] = new Piece[pieceCount[0]];
        Pieces[1] = new Piece[pieceCount[1]];
        Pieces[2] = new Piece[pieceCount[2]];
        Pieces[3] = new Piece[pieceCount[3]];
        Pieces[4] = new Piece[pieceCount[4]];
        Pieces[5] = new Piece[pieceCount[5]];
        Pieces[6] = new Piece[pieceCount[6]];
        Pieces[7] = new Piece[pieceCount[7]];

        Boards = new Board[boardCount];
        pcs = new GameObject[boardCount];

        int count = 0;

        for (int i = 0; i < pieceCount.Length; i++)
        {
            for (int j = 0; j < pieceCount[i]; j++)
            {
                Pieces[i][j] = new Piece(i);
                Boards[count] = new Board(boardDimensions);
                count += 1;
            }
        }

        basker = GameObject.FindGameObjectWithTag("Basker");
        selector = GameObject.FindGameObjectWithTag("Selector");
        rotator = GameObject.FindGameObjectWithTag("Rotator");
        translator = GameObject.FindGameObjectWithTag("Translator");

        

        state = GameState.BASKING;
        active = true;
    }

    //Here the coroutines are managed(started) if active is true
    private void FixedUpdate()
    {
        if (active)
        {
            if (state == GameState.BASKING)
            {
                StartCoroutine(Basking());
                active = false;
            }
            if (state == GameState.SELECTION)
            {
                StartCoroutine(Selecting());
                active = false;
            }
            if (state == GameState.ROTATION)
            {
                StartCoroutine(Rotating());
                active = false;
            }
            if (state == GameState.TRANSLATION)
            {
                StartCoroutine(Translating());
                active = false;
            }
        }
    }

    //Used by Basking to see if there are any pieces left
    private bool piecesLeft()
    {
        for (int i = 0; i < Pieces.Length; i++)
            for (int j = 0; j < Pieces[i].Length; j++)
                if (Pieces[i][j] != null)
                    return true;
        return false;
    }


    //Each of the individual coroutines gets the script associated, activates it and yeilds until the script makes itself inactive, then the results are stored.
    IEnumerator Basking()
    {
        Debug.Log("In Basking state");

        if (!piecesLeft() && currentTurn > 0)
        {
            GameObject destroyerOfWorlds = GameObject.Find("LevelLoader");
            LevelLoader ll = destroyerOfWorlds.GetComponent<LevelLoader>();
            Debug.Log("END OF GAME REACHED");
            ll.LoadNextLevel();
        }
        else
        {
            //// CURRENTLY TESTING
            Debug.Log("Debuging Basking");

            string matrix = "";
            bool[,,] boardMatrix;

            for (int o = 0; o < boardCount; o++)
            {
                matrix += o + " Board: \n" ;

                boardMatrix = Boards[o].BoardMatrix;

                for (int h = 0; h < 4; h++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                            matrix += h + ":" + i + ":" + j + boardMatrix[h, i, j] + " ";
                        matrix += "\n";
                    }
                    matrix += "\n";
                }
                matrix += "Piece: " + Boards[o].BoardPiece + "\n\n";
            }

            string testpath = Application.dataPath + @"\Test\baskingTest.txt";
            System.IO.File.WriteAllText(testpath, matrix);
            ////
            
            Basker bask = basker.GetComponent<Basker>();
            
            bask.undoable = false;

            if (currentTurn > 0)
                bask.undoable = true;

            bask.basking = true;

            while (bask.basking != false)
                yield return null;

            if (bask.undo == false)
            {
                state = GameState.SELECTION;
                active = true;
            }
            else
            {
                if (currentTurn > 0)
                {
                    AudioMan.Play("Undo");
                    bask.undo = false;

                    Debug.Log("UNDO PUSHED");

                    currentTurn -= 1;

                    boardMatrix = new bool[boardDimensions[0], boardDimensions[1], boardDimensions[2]];

                    if (currentTurn > 0)
                        boardMatrix = Boards[currentTurn - 1].BoardMatrix;

                    for (int i = (currentTurn); i < (Boards.Length - 1); i++)
                        Boards[i].BoardMatrix = boardMatrix.Clone() as bool[,,];

                    CurrentPiece = Boards[currentTurn].BoardPiece;

                    Boards[currentTurn].BoardPiece = null;

                    bool[,,] rotatingMatrix = CurrentPiece.RotatingMatrix;

                    CurrentPiece.TranslatingMatrix = rotatingMatrix.Clone() as bool[,,];

                    pcs[currentTurn].transform.position = CurrentPiece.Center;

                    state = GameState.TRANSLATION;
                    active = true;
                }
            }
        }
    }

    IEnumerator Selecting()
    {
        Debug.Log("In Selecting state");

        Selection select = selector.GetComponent<Selection>();
        select.Pieces = Pieces;
        select.selecting = true;

        while (select.selecting != false)
            yield return null;

        if (select.undo == false)
        {
            currentPiece = select.currentPiece;
            for (int i = (Pieces[currentPiece].Length - 1); i > -1; i--)
            {
                if (Pieces[currentPiece][i] != null)
                {
                    CurrentPiece = Pieces[currentPiece][i];
                    Pieces[currentPiece][i] = null;
                    break;
                }
            }

            state = GameState.ROTATION;
            active = true;
        }
        else
        {
            AudioMan.Play("Undo");
            Debug.Log("UNDO PUSHED");
            select.undo = false;
            state = GameState.BASKING;
            active = true;
        }
    }

    IEnumerator Rotating()
    {
        Debug.Log("In Rotating state");

        switch (CurrentPiece.Shape)
        {
            case 0:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/1c Piece") as GameObject, RotationPosition);
                break;

            case 1:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/4c Piece") as GameObject, RotationPosition);
                break;

            case 2:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/I Piece") as GameObject, RotationPosition);
                break;

            case 3:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/L Piece") as GameObject, RotationPosition);
                break;

            case 4:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/O Piece") as GameObject, RotationPosition);
                break;

            case 5:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/S Piece") as GameObject, RotationPosition);
                break;

            case 6:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/St Piece") as GameObject, RotationPosition);
                break;

            case 7:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/T Piece") as GameObject, RotationPosition);
                break;

            default:
                Debug.Log("INVALID OBJECT TAG");
                break;
        }

        Rotation rotation = rotator.GetComponent<Rotation>();
        bool[,,] rotatingMatrix = CurrentPiece.RotatingMatrix;
        rotation.RotatingMatrix = rotatingMatrix.Clone() as bool[,,];

        rotation.RotatingObject = pcs[currentTurn];
        rotation.RotatingPiece = CurrentPiece;

        rotation.Rotating = true;

        while (rotation.Rotating != false)
            yield return null;

        if (rotation.undo == false)
        {
            rotation.postRotation(boardDimensions[0], boardDimensions[2]);
            rotatingMatrix = rotation.RotatingMatrix;

            pcs[currentTurn] = rotation.RotatingObject;

            CurrentPiece = rotation.RotatingPiece;

            pcs[currentTurn].transform.position = CurrentPiece.Center;

            CurrentPiece.RotatingMatrix = rotatingMatrix.Clone() as bool[,,];
            CurrentPiece.TranslatingMatrix = rotatingMatrix.Clone() as bool[,,];
            state = GameState.TRANSLATION;
            active = true;
        }
        else
        {
            AudioMan.Play("Undo");
            Debug.Log("UNDO PUSHED");
            rotation.undo = false;

            CurrentPiece.Center = new Vector3(0f, 1.5f, 0f);

            bool[,,] originalMatrix = CurrentPiece.OriginalMatrix;
            CurrentPiece.RotatingMatrix = originalMatrix.Clone() as bool[,,];

            for (int i = (Pieces[currentPiece].Length - 1); i > -1; i--)
            {
                if (Pieces[currentPiece][i] == null)
                {
                    Pieces[currentPiece][i] = CurrentPiece;
                    break;
                }
            }

            CurrentPiece = null;

            Destroy(pcs[currentTurn]);
            pcs[currentTurn] = new GameObject();

            state = GameState.SELECTION;
            active = true;
        }
    }

    IEnumerator Translating()
    {
        Debug.Log("In Translating state");
        Translation translation = translator.GetComponent<Translation>();
        bool[,,] translatingMatrix = CurrentPiece.TranslatingMatrix;
        translation.TranslatingMatrix = translatingMatrix.Clone() as bool[,,];

        bool[,,] boardMatrix = Boards[currentTurn].BoardMatrix;
        translation.BoardMatrix = boardMatrix;

        translation.TranslatingObject = pcs[currentTurn];
        translation.Translating = true;

        while (translation.Translating != false)
            yield return null;

        if (translation.undo == false)
        {
            boardMatrix = translation.BoardMatrix;
            CurrentPiece.TranslatingMatrix = translation.TranslatingMatrix;

            Boards[currentTurn].BoardPiece = CurrentPiece;

            CurrentPiece = null;
            for (int i = (currentTurn); i < (Boards.Length); i++)
                Boards[i].BoardMatrix = boardMatrix.Clone() as bool[,,];

            pcs[currentTurn] = translation.TranslatingObject;

            currentTurn += 1;

            state = GameState.BASKING;
            active = true;
        }

        else
        {
            AudioMan.Play("Undo");
            Debug.Log("UNDO PUSHED");
            translation.undo = false;

            CurrentPiece.TranslatingMatrix = null;
            bool[,,] originalMatrix = CurrentPiece.OriginalMatrix;
            CurrentPiece.RotatingMatrix = originalMatrix.Clone() as bool[,,];
            CurrentPiece.Center = new Vector3(0f, 1.5f, 0f);

            Destroy(pcs[currentTurn]);
            pcs[currentTurn] = new GameObject();

            state = GameState.ROTATION;
            active = true;
        }
    }
}
