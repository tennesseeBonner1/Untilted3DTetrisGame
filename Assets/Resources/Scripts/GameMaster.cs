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

        //Create the Objects(not game objects) for pieces and boards
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

        //Access the scripts in these existing gameobject
        basker = GameObject.FindGameObjectWithTag("Basker");
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

    ///IENUMERATORS
    //Each of the individual coroutines gets the script associated, activates it and yeilds until the script makes itself inactive, then the results are stored. 
    IEnumerator Basking()
    {
        //If there are no pieces left and it's not the first turn don't move on 
        if (!piecesLeft() && currentTurn > 0)
        {
            GameObject destroyerOfWorlds = GameObject.Find("LevelLoader");
            LevelLoader ll = destroyerOfWorlds.GetComponent<LevelLoader>();
            ll.LoadNextLevel();
        }
        else
        {
            bool[,,] boardMatrix;

            Basker bask = basker.GetComponent<Basker>();
            
            //Don't let the player undo if it's the first turn
            bask.undoable = false;

            if (currentTurn > 0)
                bask.undoable = true;

            //Turn the basking script on and wait for it to finish
            bask.basking = true;

            while (bask.basking != false)
                yield return null;

            //Check if undo is pushed
            if (bask.undo == false)
            {
                state = GameState.SELECTION;
                active = true;
            }
            //Undo back to the state right after the previous rotating routine
            else
            {
                //Just to be safe
                if (currentTurn > 0)
                {
                    AudioMan.Play("Undo");
                    bask.undo = false;

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
        //Load in the pieces and select a piece
        Selection.Pieces = Pieces;
        Selection.selecting = true;

        while (Selection.selecting != false)
            yield return null;

        //If undo does not happen
        if (Selection.undo == false)
        {
            //Get the piece from the selection script and adjust the pieces and currentPiece accordingly
            currentPiece = Selection.currentPiece;
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
            Selection.undo = false;
            state = GameState.BASKING;
            active = true;
        }
    }

    IEnumerator Rotating()
    {
        string piecename = "Piece " + currentTurn + ": ";

        switch (CurrentPiece.Shape)
        {
            case 0:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/1c Piece") as GameObject, RotationPosition);
                piecename += "1 Corner";
                break;

            case 1:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/4c Piece") as GameObject, RotationPosition);
                piecename += "4 Corner";
                break;

            case 2:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/I Piece") as GameObject, RotationPosition);
                piecename += "I";
                break;

            case 3:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/L Piece") as GameObject, RotationPosition);
                piecename += "L";
                break;

            case 4:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/O Piece") as GameObject, RotationPosition);
                piecename += "O";
                break;

            case 5:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/S Piece") as GameObject, RotationPosition);
                piecename += "S";
                break;

            case 6:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/St Piece") as GameObject, RotationPosition);
                piecename += "Stair";
                break;

            case 7:
                pcs[currentTurn] = Instantiate(Resources.Load("Prefabs/T Piece") as GameObject, RotationPosition);
                piecename += "T";
                break;

            default:
                Debug.Log("INVALID OBJECT TAG");
                break;
        }
        pcs[currentTurn].name = piecename;

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
            pcs[currentTurn] = null;

            state = GameState.SELECTION;
            active = true;
        }
    }

    IEnumerator Translating()
    {
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
            translation.undo = false;

            CurrentPiece.TranslatingMatrix = null;
            bool[,,] originalMatrix = CurrentPiece.OriginalMatrix;
            CurrentPiece.RotatingMatrix = originalMatrix.Clone() as bool[,,];
            CurrentPiece.Center = new Vector3(0f, 1.5f, 0f);

            Destroy(pcs[currentTurn]);
            pcs[currentTurn] = null;

            state = GameState.ROTATION;
            active = true;
        }
    }
}
