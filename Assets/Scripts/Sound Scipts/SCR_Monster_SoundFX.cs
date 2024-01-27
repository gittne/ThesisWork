using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SCR_Monster_SoundFX : MonoBehaviour
{
    [SerializeField] AudioClip[] IdleSounds;
    [SerializeField] AudioSource volSounds;
    [SerializeField] AudioClip[] SpottedSounds;
    [SerializeField] AudioClip[] AngrySounds;
    [SerializeField] float waitInterwall;
    [SerializeField] float repInterwall;
    [SerializeField] float startRepeating;
    bool canPlayNewSounds = true;
    float Thold = 0;


    SCR_EnemyBrain brain;

    void Start()
    {
        brain = GetComponent<SCR_EnemyBrain>();
        Thold = UnityEngine.Random.Range(0, 55); // Thold is give a value between 1 and 55. 
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlayNewSounds) return;
        StartCoroutine(SoundDelay());

        if (brain.GetRageAmount() < 30)
        {
            IdleSoundManager();
        }
        else if (brain.GetRageAmount() < 80 && brain.GetRageAmount() > 30)
        {
            FollowingSoundManager();

        }
        else if (brain.GetRageAmount() > 80)
        {
            AngrySoundManager();
        }
    }

    void IdleSoundManager()
    {

        if (Thold > waitInterwall) // will only play sound if thold is larger then waitinterwall. Problem, thold value is only created at start, meaning if value is under X, sound will never play. 
        {
            volSounds.PlayOneShot(IdleSounds[Random.Range(0, IdleSounds.Length - 1)]);
    
        }

    }

    void FollowingSoundManager()
    {

        if (Thold > waitInterwall)
        {
            volSounds.PlayOneShot(SpottedSounds[Random.Range(0, SpottedSounds.Length - 1)]);
           
        }

    }

    void AngrySoundManager()
    {

        if (Thold > waitInterwall)
        {
            volSounds.PlayOneShot(AngrySounds[Random.Range(0, AngrySounds.Length - 1)]);
           
        }

    }

    IEnumerator SoundDelay()
    {
        canPlayNewSounds = false;
        yield return new WaitForSeconds(5);
        canPlayNewSounds = true;
    }

    void KillSound()
    {

    }


}
