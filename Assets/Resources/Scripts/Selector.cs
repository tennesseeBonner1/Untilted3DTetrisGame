using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Selector : MonoBehaviour
{
    public static bool selecting;
    public static int currentPiece;
    public static bool reset;

    private PlayerControls controls;

    private GameObject peiceSelectionMenuUI;
    private GameObject psmFirstButton;

    private GameState startState;
    private int pausePiece;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Select.started += ctx => Pause();
        controls.Gameplay.Select.canceled += ctx => Resume();

        peiceSelectionMenuUI = GameObject.FindGameObjectWithTag("PieceSelection");
        EventSystem.current.SetSelectedGameObject(psmFirstButton);
        startState = GameState.BASKING;

        pausePiece = 0;
        currentPiece = 0;
        reset = false;
        selecting = true;
        Resume();
    }

    public void Resume()
    {
        if (selecting)
        {
            if (pausePiece != currentPiece && GameMaster.state != GameState.BASKING)
                reset = true;

            peiceSelectionMenuUI.SetActive(false);
            Time.timeScale = 1f;
            selecting = false;
        }
    }


    void Pause()
    {
        pausePiece = currentPiece;

        if (!PauseMenu.GameIsPaused)
        {
            peiceSelectionMenuUI.SetActive(true);
            Time.timeScale = 0f;

            switch (currentPiece)
            {
                case (0):
                    psmFirstButton = peiceSelectionMenuUI.transform.Find("1CButton").gameObject;
                    break;
                case (1):
                    psmFirstButton = peiceSelectionMenuUI.transform.Find("4CButton").gameObject;
                    break;
                case (2):
                    psmFirstButton = peiceSelectionMenuUI.transform.Find("IButton").gameObject;
                    break;
                case (3):
                    psmFirstButton = peiceSelectionMenuUI.transform.Find("LButton").gameObject;
                    break;
                case (4):
                    psmFirstButton = peiceSelectionMenuUI.transform.Find("OButton").gameObject;
                    break;
                case (5):
                    psmFirstButton = peiceSelectionMenuUI.transform.Find("SButton").gameObject;
                    break;
                case (6):
                    psmFirstButton = peiceSelectionMenuUI.transform.Find("StButton").gameObject;
                    break;
                case (7):
                    psmFirstButton = peiceSelectionMenuUI.transform.Find("TButton").gameObject;
                    break;
            }

            selecting = true;
        }
    }

    public void On1CButton()
    {
        if (GameMaster.pieceCount[0] == 0)
            return;
        Basker.basking = false;
        currentPiece = 0;
        Resume();
    }
    public void On4CButton()
    {
        if (GameMaster.pieceCount[1] == 0)
            return;
        Basker.basking = false;
        currentPiece = 1;
        Resume();
    }
    public void IButton()
    {
        if (GameMaster.pieceCount[2] == 0)
            return;
        Basker.basking = false;
        currentPiece = 2;
        Resume();
    }
    public void LButton()
    {
        if (GameMaster.pieceCount[3] == 0)
            return;
        Basker.basking = false;
        currentPiece = 3;
        Resume();
    }
    public void OButton()
    {
        if (GameMaster.pieceCount[4] == 0)
            return;
        Basker.basking = false;
        currentPiece = 4;
        Resume();
    }
    public void SButton()
    {
        if (GameMaster.pieceCount[5] == 0)
            return;
        Basker.basking = false;
        currentPiece = 5;
        Resume();
    }
    public void StButton()
    {
        if (GameMaster.pieceCount[6] == 0)
            return;
        Basker.basking = false;
        currentPiece = 6;
        Resume();
    }
    public void TButton()
    {
        if (GameMaster.pieceCount[7] == 0)
            return;
        Basker.basking = false;
        currentPiece = 7;
        Resume();
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
