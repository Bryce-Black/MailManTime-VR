using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource music;
    private static MusicManager instance;
    private bool musicPlaying = true;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    public void ToggleMusic()
    {
        if(musicPlaying)
        {
            musicPlaying = false;
            music.enabled = false;
        }
        else
        {
            musicPlaying = true;
            music.enabled = true;
        }
    }
}
