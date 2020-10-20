//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 12, 2020
//
//Selection.cs
//Used to run a Selection routine for the player to select what piece they want.
//Public values are controlled in GameMaster to make some buttons unusable in the selection.
//GameMaster also will use the value determined during the selection routine to use in the game.
using UnityEngine;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{
    public static bool selecting;//If selecting routine is active
    public static bool undo;     //If undo is pushed

    private bool done;//If the menu has been opened 

    public static int currentPiece;//The piece that is selected
      
    private GameObject peiceSelectionMenuUI;//The canvas for the pause menu  

    public static Piece[][] Pieces;//The pieces array used to tell what's left

    public GameObject psmFirstButton;//The first button selected in the pause menu

    private PlayerControls controls;//The platercontrols

    //Get the controls, set up the boolean values and close the pause menu by default
    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Undo.performed += ctx => Undo();

        selecting = false;
        done = false;
        undo = false;

        currentPiece = -1;

        peiceSelectionMenuUI = GameObject.FindGameObjectWithTag("PieceSelection");
        peiceSelectionMenuUI.SetActive(false);
    }

    //Activates the piece Selection menu when selecting is true and exits if undo is pushed
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (selecting && !done)
            {
                peiceSelectionMenuUI.SetActive(true);
                Time.timeScale = 0f;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(psmFirstButton);

                done = true;
            }
        }
        else
        {
            peiceSelectionMenuUI.SetActive(false);
            done = false;
        }
    }

    //If undo is pushed during selection
    void Undo()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (selecting)
            {
                peiceSelectionMenuUI.SetActive(false);
                Time.timeScale = 1f;
                undo = true;
                selecting = false;
                done = false;
            }
        }
    }

    //All the possible buttons for pieces that can be selected
    public void On1CButton()
    {
        if (Pieces[0].Length < 1)
            return;
        if (Pieces[0][0] == null)
            return;

        currentPiece = 0;

        peiceSelectionMenuUI.SetActive(false);
        Time.timeScale = 1f;

        selecting = false;
        done = false;
    }
    public void On4CButton()
    {
        if (Pieces[1].Length < 1)
            return;
        if (Pieces[1][0] == null)
            return;

        currentPiece = 1;

        peiceSelectionMenuUI.SetActive(false);
        Time.timeScale = 1f;

        selecting = false;
        done = false;
    }
    public void IButton()
    {
        if (Pieces[2].Length < 1)
            return;
        if (Pieces[2][0] == null)
            return;

        currentPiece = 2;

        peiceSelectionMenuUI.SetActive(false);
        Time.timeScale = 1f;

        selecting = false;
        done = false;
    }
    public void LButton()
    {
        if (Pieces[3].Length < 1)
            return;
        if (Pieces[3][0] == null)
            return;

        currentPiece = 3;

        peiceSelectionMenuUI.SetActive(false);
        Time.timeScale = 1f;

        selecting = false;
        done = false;
    }
    public void OButton()
    {
        if (Pieces[4].Length < 1)
            return;
        if (Pieces[4][0] == null)
            return;

        currentPiece = 4;

        peiceSelectionMenuUI.SetActive(false);
        Time.timeScale = 1f;

        selecting = false;
        done = false;
    }
    public void SButton()
    {
        if (Pieces[5].Length < 1)
            return;
        if (Pieces[5][0] == null)
            return;
        currentPiece = 5;

        peiceSelectionMenuUI.SetActive(false);
        Time.timeScale = 1f;

        selecting = false;
        done = false;
    }
    public void StButton()
    {
        if (Pieces[6].Length < 1)
            return;
        if (Pieces[6][0] == null)
            return;

        currentPiece = 6;

        peiceSelectionMenuUI.SetActive(false);
        Time.timeScale = 1f;

        selecting = false;
        done = false;
    }
    public void TButton()
    {
        if (Pieces[7].Length < 1)
            return;
        if (Pieces[7][0] == null)
            return;

        currentPiece = 7;

        peiceSelectionMenuUI.SetActive(false);
        Time.timeScale = 1f;

        selecting = false;
        done = false;
    }

    //Required for the controls
    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
