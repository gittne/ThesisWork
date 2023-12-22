using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MusicPlaylist : MonoBehaviour
{

    private static SCR_MusicPlaylist _instance;
    public static SCR_MusicPlaylist Instance
    {
        get
        {
            return _instance;
        }
    }
    private AudioSource musicSong;
    [SerializeField] AudioClip[] songs;
    private float trackTimer;
    private float playedSongs;
    private bool[] hasBeenPlayed;
    int previousSong;
    int currentSong;



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        musicSong = GetComponent<AudioSource>();

        hasBeenPlayed = new bool[songs.Length];

        if (!musicSong.isPlaying)
        {
            currentSong = Random.Range(0, songs.Length);
            previousSong = currentSong;
            ChangeSong(currentSong);
            Debug.Log(currentSong);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (musicSong.isPlaying)
        {
            trackTimer += 1 * Time.deltaTime;
        }

        if (!musicSong.isPlaying || trackTimer >= musicSong.clip.length)
        {
            currentSong = Random.Range(0, songs.Length);
            if (currentSong != previousSong)
            {
                ChangeSong(Random.Range(0, songs.Length));
                previousSong = currentSong;
            }
            // ChangeSong(Random.Range(0, songs.Length));
        }
        ResetSongShuffle();

    }


    public void ChangeSong(int songCurrent) // changes songs
    {
        if (!hasBeenPlayed[songCurrent])
        {
            trackTimer = 0;
            playedSongs++;
            hasBeenPlayed[songCurrent] = true;
            musicSong.clip = songs[songCurrent];
            musicSong.Play();
        }
        else
        {
            musicSong.Stop();
        }
    }


    private void ResetSongShuffle()
    {
        if (playedSongs == songs.Length)
        {
            playedSongs = 0;
            for (int i = 0; i < songs.Length; i++)
            {
                if (i == songs.Length)
                {
                    break;
                }
                else
                {
                    hasBeenPlayed[i] = false;
                }
            }
        }
    }

    //Sets the volume from settings
    //public void LoadVolume()
    //{
    //    AudioSource[] soundMakers = GameObject.FindObjectsOfType<AudioSource>();

    //    foreach (AudioSource sound in soundMakers)
    //    {
    //        sound.volume = S_GameSettings.currentVolume;
    //    }
    //}
}
