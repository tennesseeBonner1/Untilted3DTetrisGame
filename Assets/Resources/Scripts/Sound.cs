//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 12, 2020
//
//Setup.cs
//Contains the Setup class which is used to setup a game
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string name;//Name of the sound

    public AudioClip clip;//The sound clip

    [Range(0f, 1f)]
    public float volume;//Volume of the sound
    [Range(.1f, 3f)]
    public float pitch;//pitch of the sound

    [HideInInspector]
    public AudioSource source;//Source of the clip

    public bool loop;//Should it loop?
}
