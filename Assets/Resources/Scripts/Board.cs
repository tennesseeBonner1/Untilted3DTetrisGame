//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 11, 2020
//
//Board.cs
//Contains the Board class which is used in PackItUpPackItIn.
//Board is controlled by the GameMaster, where multiple boards are created.
using System.Collections;
using UnityEngine;

public class Board 
{
    //The piece object the board holds
    private Piece boardPiece = null;

    //The matrix representation of the board
    private bool[,,] boardMatrix =
    {
                              {{ false, false, false, false },
                               { false, false, false, false },
                               { false, false, false, false },
                               { false, false, false, false }},

                              {{ false, false, false, false },
                               { false, false, false, false },
                               { false, false, false, false },
                               { false, false, false, false }},

                              {{ false, false, false, false },
                               { false, false, false, false },
                               { false, false, false, false },
                               { false, false, false, false }},

                              {{ false, false, false, false },
                               { false, false, false, false },
                               { false, false, false, false },
                               { false, false, false, false }},
    };


    //Board Constructor(creates a board of the input size)
    public Board(int[] boardDimensions)
    {
        boardMatrix = new bool[boardDimensions[0], boardDimensions[1], boardDimensions[2]];
    }


    //Getters and setters so the private values are protected by a layer of abstraction 
    public Piece BoardPiece
    {
        get
        {
            return boardPiece;
        }
        set
        {
            boardPiece = value;
        }
    }
    public bool[,,] BoardMatrix
    {
        get
        {
            return boardMatrix;
        }
        set
        {
            boardMatrix = value;
        }
    }
}

