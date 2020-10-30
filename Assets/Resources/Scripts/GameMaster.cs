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
    public AudioManager AudioMan;//Audiomanager

    public static int[] boardDimensions;//Dimensions of the board
    public static int[] pieceCount;//Number of pieces in a 1D array

    private Piece[][] Pieces;//Piece Objects(for data)
    private Board[] Boards;//Board Objects(for data)

    private GameObject[] PieceGameOb;

    private static int currentTurn;//Index for thew current turn
    public static GameState state;//The state the game is in
    private bool active;//If the update function should be active(not in a Coroutine)


    //Create and initialize the piece and board objects and start the basking state
    private void Awake()
    {
        //Get the audio manager
        GameObject am = GameObject.Find("AudioManager");
        AudioMan = am.GetComponent<AudioManager>();

        //Get the Setup
        boardDimensions = Setup.startBoard;
        pieceCount =  Setup.pieces;
        int turnCount =  Setup.getCount();

        //Create the Piece objects
        Pieces = new Piece[pieceCount.Length][];
        Pieces[0] = new Piece[pieceCount[0]];
        Pieces[1] = new Piece[pieceCount[1]];
        Pieces[2] = new Piece[pieceCount[2]];
        Pieces[3] = new Piece[pieceCount[3]];
        Pieces[4] = new Piece[pieceCount[4]];
        Pieces[5] = new Piece[pieceCount[5]];
        Pieces[6] = new Piece[pieceCount[6]];
        Pieces[7] = new Piece[pieceCount[7]];

        //Create the board objects
        Boards = new Board[turnCount];

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

        //Create the GameObjects for the pieces
        PieceGameOb = new GameObject[turnCount];

        //It's turn zero and you start at basking
        currentTurn = 0;

        Basker.basking = true;
        state = GameState.BASKING;
        active = true;
    }


    //Here the coroutines are managed(started) if active is true
    private void Update()
    {
        if (active)
        {
            if (state == GameState.BASKING)
            {
                StartCoroutine(Basking());
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

        if(!Selector.selecting && Selector.reset)
        {
            Debug.Log("RESET");
            UndoPiece();
        }
    }

    private Piece CurrentPiece;
    public Transform RotationPosition;

    IEnumerator Basking()
    {
        Debug.Log("Basking Started");
        testing();

        //Setup and start Basking routine
        Basker.undoable = false;
        if (currentTurn > 0)
            Basker.undoable = true;

        //Wait for basking Routine to finish
        while (Basker.basking != false)
            yield return null;

        //If undo is NOT pushed
        if (Basker.undo == false)
        {
            //Pick a piece based on the selector pick, but tweak it if we're out of the piece
            while (pieceCount[Selector.currentPiece] == 0)
            {
                Selector.currentPiece += 1;
                if (Selector.currentPiece == 8)
                    Selector.currentPiece = 0;
            }

            //Drop the count index for that piece
            pieceCount[Selector.currentPiece] -= 1;

            //Change CurrentPiece to match the shape of said piece
            CurrentPiece = new Piece(Pieces[Selector.currentPiece][0].Shape);

            //Create a piece gameObject of that type and name
            string piecename = "Piece " + currentTurn + ": ";
            switch (CurrentPiece.Shape)
            {
                case 0:
                    PieceGameOb[currentTurn] = Instantiate(Resources.Load("Prefabs/1c Piece") as GameObject, RotationPosition);
                    piecename += "1 Corner";
                    break;

                case 1:
                    PieceGameOb[currentTurn] = Instantiate(Resources.Load("Prefabs/4c Piece") as GameObject, RotationPosition);
                    piecename += "4 Corner";
                    break;

                case 2:
                    PieceGameOb[currentTurn] = Instantiate(Resources.Load("Prefabs/I Piece") as GameObject, RotationPosition);
                    piecename += "I";
                    break;

                case 3:
                    PieceGameOb[currentTurn] = Instantiate(Resources.Load("Prefabs/L Piece") as GameObject, RotationPosition);
                    piecename += "L";
                    break;

                case 4:
                    PieceGameOb[currentTurn] = Instantiate(Resources.Load("Prefabs/O Piece") as GameObject, RotationPosition);
                    piecename += "O";
                    break;

                case 5:
                    PieceGameOb[currentTurn] = Instantiate(Resources.Load("Prefabs/S Piece") as GameObject, RotationPosition);
                    piecename += "S";
                    break;

                case 6:
                    PieceGameOb[currentTurn] = Instantiate(Resources.Load("Prefabs/St Piece") as GameObject, RotationPosition);
                    piecename += "Stair";
                    break;

                case 7:
                    PieceGameOb[currentTurn] = Instantiate(Resources.Load("Prefabs/T Piece") as GameObject, RotationPosition);
                    piecename += "T";
                    break;

                default:
                    Debug.Log("INVALID OBJECT TAG");
                    break;
            }
            PieceGameOb[currentTurn].name = piecename;

            state = GameState.ROTATION;
            active = true;
        }
        else
        {
            //Just to be safe
            if (currentTurn > 0)
            {
                UndoBask();
                state = GameState.TRANSLATION;
                active = true;
            }
        }
        
    }

    public void UndoBask()
    {
        StopAllCoroutines();
        //Play undo sound effect
        AudioMan.Play("Undo");

        //undo what translation routine does
        currentTurn -= 1;
        CurrentPiece = Boards[currentTurn].BoardPiece;

        //Reset this and all proceeding boards to either a blank board or the board of currentturn-1
        bool[,,] boardMatrix = new bool[boardDimensions[0], boardDimensions[1], boardDimensions[2]];

        if (currentTurn > 0)
            boardMatrix = Boards[currentTurn - 1].BoardMatrix;

        for (int i = (currentTurn); i < (Boards.Length); i++)
            Boards[i].BoardMatrix = boardMatrix.Clone() as bool[,,];

        Boards[currentTurn].BoardPiece = null;

        bool[,,] translationMatrix = CurrentPiece.RotatingMatrix;
        CurrentPiece.TranslatingMatrix = translationMatrix.Clone() as bool[,,];
        PieceGameOb[currentTurn].transform.position = CurrentPiece.Center;

        Basker.basking = false;

        //Reset undo for basker
        Basker.undo = false;
    }

    IEnumerator Rotating()
    {
        Debug.Log("Rotating Started");
        //Setup and start Rotation routine
        bool[,,] rotatingMatrix = CurrentPiece.RotatingMatrix;
        Rotation.RotatingMatrix = rotatingMatrix.Clone() as bool[,,];
        Rotation.RotatingObject = PieceGameOb[currentTurn];
        Rotation.RotatingPiece = CurrentPiece;
        Rotation.Rotating = true;

        //Wait for rotation Routine to finish
        while (Rotation.Rotating != false)
            yield return null;

        //If undo is NOT pushed
        if (Rotation.undo == false)
        {
            //Get information from Rotation
            rotatingMatrix = Rotation.RotatingMatrix;
            PieceGameOb[currentTurn] = Rotation.RotatingObject;
            CurrentPiece = Rotation.RotatingPiece;

            //Move GameObject
            PieceGameOb[currentTurn].transform.position = CurrentPiece.Center;

            //Get rotating and translating matricies
            CurrentPiece.RotatingMatrix = rotatingMatrix.Clone() as bool[,,];
            CurrentPiece.TranslatingMatrix = rotatingMatrix.Clone() as bool[,,];

            state = GameState.TRANSLATION;
            active = true;
        }
        else
        {
            UndoRotation();
            Basker.basking = true;
            state = GameState.BASKING;
            active = true;
        }
    }

    public void UndoRotation()
    {
        StopAllCoroutines();

        //Play undo sound effect 
        AudioMan.Play("Undo");

        //Reset all values at begining of rotation routine
        Rotation.RotatingMatrix = null;
        Rotation.RotatingObject = null;
        Rotation.RotatingPiece = null;
        Rotation.Rotating = false;

        //Undo what basking does 
        Destroy(PieceGameOb[currentTurn]);
        PieceGameOb[currentTurn] = null;
        pieceCount[CurrentPiece.Shape] += 1;
        CurrentPiece = null;

        //Reset undo for rotation
        Rotation.undo = false;
    }

    IEnumerator Translating()
    {
        Debug.Log("Translating Started");
        testing();
        //Setup and start translation routine
        bool[,,] translatingMatrix = CurrentPiece.TranslatingMatrix;
        Translation.TranslatingMatrix = translatingMatrix.Clone() as bool[,,];
        bool[,,] boardMatrix = Boards[currentTurn].BoardMatrix;
        Translation.BoardMatrix = boardMatrix;
        Translation.TranslatingObject = PieceGameOb[currentTurn];
        Translation.Translating = true;

        while (Translation.Translating != false)
            yield return null;

        if (Translation.undo == false)
        {
            //Get board matrix, completed piece and piece Gameobject
            boardMatrix = Translation.BoardMatrix;
            CurrentPiece.TranslatingMatrix = Translation.TranslatingMatrix;
            PieceGameOb[currentTurn] = Translation.TranslatingObject;

            //Give the board object the correct piece
            Boards[currentTurn].BoardPiece = CurrentPiece;

            //Clone the board matrix for every preceeding turn
            for (int i = (currentTurn); i < (Boards.Length); i++)
                Boards[i].BoardMatrix = boardMatrix.Clone() as bool[,,];

            //Nullify CurrentPiece and increase turn by one
            CurrentPiece = null;
            currentTurn += 1;

            //If there are no pieces left and it's not the first turn don't move on 
            if (!piecesLeft() && currentTurn > 0)
            {
                GameObject destroyerOfWorlds = GameObject.Find("LevelLoader");
                LevelLoader ll = destroyerOfWorlds.GetComponent<LevelLoader>();
                ll.LoadNextLevel();
            }

            Basker.basking = true;
            state = GameState.BASKING;
            active = true;
        }

        else
        {
            UndoTranslation();
            state = GameState.ROTATION;
            active = true;
        }
    }

    public void UndoTranslation()
    {
        StopAllCoroutines();
        //Play undo sound effect
        AudioMan.Play("Undo");

        //Undo what translation routine has done
        CurrentPiece.TranslatingMatrix = null;

        bool[,,] rotatingMatrix = CurrentPiece.OriginalMatrix;
        CurrentPiece.RotatingMatrix = rotatingMatrix.Clone() as bool[,,];

        PieceGameOb[currentTurn].transform.position = new Vector3(0f, 1.5f, 0f);
        PieceGameOb[currentTurn].transform.eulerAngles = new Vector3(0f, 0f, 0f);
        CurrentPiece.Center = new Vector3(0f, 1.5f, 0f);

        //Reset all values at begining of translation routine
        Translation.TranslatingMatrix = null;
        Translation.BoardMatrix = null;
        Translation.TranslatingObject = null;
        Translation.Translating = false;

        //Reset undo for translation
        Translation.undo = false;
    }

    void UndoPiece()
    {
        Selector.reset = false;
        Basker.basking = false;

        switch (state)
        {
            case (GameState.ROTATION):
                UndoRotation();
                state = GameState.BASKING;
                active = true;
                break;
            case (GameState.TRANSLATION):
                UndoTranslation();
                UndoRotation();
                state = GameState.BASKING;
                active = true;
                break;
        }
    }

    //Used by Translation to see if there are any pieces left
    private bool piecesLeft()
    {
        for (int i = 0; i < pieceCount.Length; i++)
            if (pieceCount[i] != 0)
                return true;
        return false;
    }

    void testing()
    {
        string matrix = state + " " + currentTurn +" State board is \n";
        bool[,,] boardMatrix;

        for (int o = 0; o < Boards.Length; o++)
        {
            matrix += o + " Board: \n";

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
            if (Boards[o].BoardPiece != null)
                matrix += "Piece: " + Boards[o].BoardPiece.Shape + "\n\n";
        }

        matrix += "==========================================================================\n\nCurrentPiece: ";

        if (CurrentPiece != null)
        {
            matrix += CurrentPiece.Shape + "\n";

            matrix += "RotatingMatrix: \n";
            if (CurrentPiece.RotatingMatrix != null)
            {
                boardMatrix = CurrentPiece.RotatingMatrix;

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
            }
            else
                matrix += "null\n";
        }
        else
            matrix += "null\n";

        string testpath = Application.dataPath + @"\Test\" + state + "State"  + currentTurn + ".txt";
        System.IO.File.WriteAllText(testpath, matrix);
    }

    

    
}
