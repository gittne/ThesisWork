using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SCR_Monster_SoundFX : MonoBehaviour
{
    [SerializeField] AudioClip[] IdleSounds;
    [SerializeField] AudioSource volSounds;
    [SerializeField] AudioClip[] SpottedSounds;
    [SerializeField] AudioClip[] TriggerdSounds;
    [SerializeField] AudioClip[] AngrySounds;
    [SerializeField] float waitInterwall;
    [SerializeField] float repInterwall;
    [SerializeField] float startRepeating;
    private bool gameIsRunning = true;

   
    void Start()
    {
        InvokeRepeating("IdleSoundS", startRepeating, repInterwall);

    }

    // Update is called once per frame
    void Update()
    {
        

    }

     void IdleSoundS()
    {
        if (gameIsRunning == true)
        {
            
            float Thold = Random.Range(0, 101);
            if (Thold > waitInterwall)
            {
                volSounds.PlayOneShot(IdleSounds[Random.Range(0, IdleSounds.Length - 1)]);
            }
        }
        
    }
}
