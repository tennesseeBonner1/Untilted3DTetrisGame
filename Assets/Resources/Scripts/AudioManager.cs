//Tennessee Bonner
//tennessee.bonner@protonmail.com
//https://github.com/tennesseeBonner1
//September 11, 2020
//
//AudioManager.cs
//Controls all audio in the game
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;                  //array of sound objects          

    public static AudioManager Instance;    //Instance of the manager itself

    void Awake()
    {
        //If there is no instance of an audiomanager, there is now 
        if (Instance == null)
            Instance = this;

        //There can only be one
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //Get every sound(it's clip, volume, pitch and loop)
        foreach(Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //Play the Theme to start
    public void Start()
    {
        Play("Theme");
    }

    //Play the sound passed
    public void Play (string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        s.source.Play();
    }
}
