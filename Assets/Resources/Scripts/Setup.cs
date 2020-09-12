//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 12, 2020
//
//Setup.cs
//Contains the Setup class which is used to setup a game
using UnityEngine;

public class Setup : MonoBehaviour
{
    //The initial board and piece
    public static int[] startBoard = { 4, 4, 4};               
    public static int[] pieces = { 1, 0, 2, 5, 1, 2, 2, 3 };

    public static Setup Instance;

    void Awake()
    {
        //If there's no setup, there is now
        if (Instance == null)
            Instance = this;

        //There can be only one
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    

    //Returns the number of pieces
    public static int getCount()
    {
        int count = 0; 

        for (int i = 0; i < pieces.Length; i++)
            count += pieces[i];

        return count;
    }

}