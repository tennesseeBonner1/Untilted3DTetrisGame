//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 9, 2020
//
//Setup.cs
//Contains the Setup class which is used in PackItUpPackItIn.
using System.Collections;
using UnityEngine;

public class Setup : MonoBehaviour
{
    public static int[] startBoard = { 4, 4, 4};               
    public static int[] pieces = { 1, 0, 2, 5, 1, 2, 2, 3 };

    public static Setup Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
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