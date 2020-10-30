using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeLevel : MonoBehaviour
{
    public static int[] startBoard = { 4, 4, 4};
    public static int[] pieces = { 0, 0, 0, 0, 0, 0, 0, 0 };

    public Text xText;
    public Text yText;
    public Text zText;

    public Text oneText;
    public Text fourText;
    public Text iText;
    public Text lText;
    public Text oText;
    public Text sText;
    public Text stText;
    public Text tText;

    private GameObject destroyerOfWorlds;//The level loader

    private void Awake()
    {
        destroyerOfWorlds = new GameObject();
        destroyerOfWorlds = GameObject.Find("LevelLoader");
    }

    private void Update()
    {
        xText.text = startBoard[0].ToString();
        yText.text = startBoard[1].ToString();
        zText.text = startBoard[2].ToString();

        oneText.text = pieces[0].ToString();
        fourText.text = pieces[1].ToString();
        iText.text = pieces[2].ToString();
        lText.text = pieces[3].ToString();
        oText.text = pieces[4].ToString();
        sText.text = pieces[5].ToString();
        stText.text = pieces[6].ToString();
        tText.text = pieces[7].ToString();
    }

    public void increaseX()
    {
        startBoard[0] += 1;
    }

    public void decreaseX()
    {
        if(startBoard[0] > 4)
            startBoard[0]-= 1;
    }

    public void increaseY()
    {
        startBoard[1] += 1;
    }

    public void decreaseY()
    {
        if (startBoard[1] > 4)
            startBoard[1] -= 1;
    }

    public void increaseZ()
    {
        startBoard[2] += 1;
    }

    public void decreaseZ()
    {
        if (startBoard[2] > 4)
            startBoard[2] -= 1;
    }

    public void increaseOneC()
    {
        pieces[0] += 1;
    }

    public void increaseFourC()
    {
        pieces[1] += 1;
    }

    public void increaseI()
    {
        pieces[2] += 1;
    }

    public void increaseL()
    {
        pieces[3] += 1;
    }

    public void increaseO()
    {
        pieces[4] += 1;
    }

    public void increaseS()
    {
        pieces[5] += 1;
    }

    public void increaseSt()
    {
        pieces[6] += 1;
    }

    public void increaseT()
    {
        pieces[7] += 1;
    }

    public void decreaseOneC()
    {
        if (pieces[0] > 0)
            pieces[0] -= 1;
    }

    public void decreaseFourC()
    {
        if (pieces[1] > 0)
            pieces[1] -= 1;
    }

    public void decreaseI()
    {
        if (pieces[2] > 0)
            pieces[2] -= 1;
    }
    public void decreaseL()
    {
        if (pieces[3] > 0)
            pieces[3] -= 1;
    }
    public void decreaseO()
    {
        if (pieces[4] > 0)
            pieces[4] -= 1;
    }
    public void decreaseS()
    {
        if (pieces[5] > 0)
            pieces[5] -= 1;
    }
    public void decreaseSt()
    {
        if (pieces[6] > 0)
            pieces[6] -= 1;
    }
    public void decreaseT()
    {
        if (pieces[7] > 0)
            pieces[7] -= 1;
    }

    public void Test()
    {
        for (int i = 0; i < 3; i++)
            Setup.startBoard[i] = startBoard[i];

        for (int i = 0; i < 8; i++)
            Setup.pieces[i] = pieces[i];

        LevelLoader ll = destroyerOfWorlds.GetComponent<LevelLoader>();
        ll.LoadNextLevel();
    }
}
