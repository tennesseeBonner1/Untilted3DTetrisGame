//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//August 30, 2020
//
//Piece.cs
//Contains the Piece class which is used in PackItUpPackItIn.
//This Class contains private variables, a constuctor, and getters and setters for public variables.
//Piece is controlled by the GameMaster, where multiple pieces are created; Board can also hold a piece object.
using System.Collections;
using UnityEngine;

public class Piece
{
    //Three representations of the piece so moves can be undone
    private bool[,,] originalMatrix = new bool[5, 5, 5];
    private bool[,,] rotatingMatrix = new bool[5, 5, 5];       
    private bool[,,] translatingMatrix = null;                 

    //The shape of the piece
    private int shape;

    private Vector3 center = new Vector3(0f, 1.5f, 0f);        

    //The Matricies all pieces should have initially 
    private bool[,,] CPosition5 = new bool[,,] {           {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, true, false, true, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, true, false },
                                                        { false, false, false, false, false },
                                                        { false, true, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }}, };
    private bool[,,] fourCPosition5 = new bool[,,]{        {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, true, false },
                                                        { false, false, false, false, false },
                                                        { false, true, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, true, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, true, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }}, };
    private bool[,,] IPosition5 = new bool[,,] {       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }}, };
    private bool[,,] LPosition5 = new bool[,,] {           {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                      {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, true, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }}, };
    private bool[,,] OPosition5 = new bool[,,] {           {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, true, false, true, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, true, false, true, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }}, };
    private bool[,,] SPosition5 = new bool[,,] {           {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, true, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, true, false, true, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, true, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }}, };
    private bool[,,] StPosition5 = new bool[,,] {          {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, true, false },
                                                        { false, false, false, false, false },
                                                        { false, true,  false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, true, false },
                                                        { false, false, false, false, false },
                                                        { false, true, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }}, };
    private bool[,,] TPosition5 = new bool[,,] {           {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, true, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, true, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }},

                                                       {{ false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false },
                                                        { false, false, false, false, false }}, };


    //Piece Constructor (get the piece type and change the shape and rotation/original matricies to reflect this)
    public Piece(int pieceType)
    {
        shape = pieceType;

        switch (pieceType)
        {
            case 0:
                rotatingMatrix = CPosition5.Clone() as bool[,,];
                originalMatrix = CPosition5.Clone() as bool[,,];
                break;

            case 1:
                rotatingMatrix = fourCPosition5.Clone() as bool[,,];
                originalMatrix = fourCPosition5.Clone() as bool[,,];
                break;

            case 2:
                rotatingMatrix = IPosition5.Clone() as bool[,,];
                originalMatrix = IPosition5.Clone() as bool[,,];
                break;

            case 3:
                rotatingMatrix = LPosition5.Clone() as bool[,,];
                originalMatrix = LPosition5.Clone() as bool[,,];
                break;

            case 4:
                rotatingMatrix = OPosition5.Clone() as bool[,,];
                originalMatrix = OPosition5.Clone() as bool[,,];
                break;

            case 5:
                rotatingMatrix = SPosition5.Clone() as bool[,,];
                originalMatrix = SPosition5.Clone() as bool[,,];
                break;

            case 6:
                rotatingMatrix = StPosition5.Clone() as bool[,,];
                originalMatrix = StPosition5.Clone() as bool[,,];
                break;

            case 7:
                rotatingMatrix = TPosition5.Clone() as bool[,,];
                originalMatrix = TPosition5.Clone() as bool[,,];
                break;

            default:
                Debug.Log("INVALID OBJECT TAG");
                break;
        }
    }


    //Getters and setters so the private values are protected by a layer of abstraction 
    public bool[,,] OriginalMatrix
    {
        get
        {
            return originalMatrix;
        }
    }
    public bool[,,] RotatingMatrix
    {
        get
        {
            return rotatingMatrix;
        }
        set
        {
            rotatingMatrix = value;
        }
    }
    public bool[,,] TranslatingMatrix
    {
        get
        {
            return translatingMatrix;
        }
        set
        {
            translatingMatrix = value;
        }
    }
    public int Shape
    {
        get
        {
            return shape;
        }
    }
    
    public Vector3 Center
    {
        get
        {
            return center;
        }
        set
        {
            center = value;
        }
    }
    
}