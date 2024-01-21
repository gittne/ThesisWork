using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Written by Ben Schelhaas by the help of :https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/
public class SCR_Monster_Music : MonoBehaviour
{
    // Gameobjects that will be the songs thme self.
    // This part works with SCR_Enemy Brain 
    [SerializeField] AudioSource sourceSpotted;
    [SerializeField] AudioClip clipSpotted;
    [SerializeField] AudioSource sourceTriggerd;
    [SerializeField] AudioClip clipTriggerd;
    [SerializeField] AudioSource sourceAngry;
    [SerializeField] AudioClip clipAngry;
    [SerializeField] AudioSource sourceChasing;
    [SerializeField] AudioClip clipChasing;

    void update()
    {
        PlayerSpotted();
    }

    private void PlayerSpotted()
    {
        //if (true)
        //{
        //    StartCoroutine(FadeAudioSource.StartFade(AudioSource audioSource, float duration, float targetVolume));
        //    Spotted.Play(clipSpotted);
        //}
         
    }

    private void MonsterTriggerd()
    {
        //if (true)
        //{
        //    StartCoroutine(FadeAudioSource.StartFade(AudioSource audioSource, float duration, float targetVolume));
        //    Triggerd.Play(clipTriggerd);
        //}

    }

    private void MonsterAngry()
    {
        //if (true)
        //{
        //    StartCoroutine(FadeAudioSource.StartFade(AudioSource audioSource, float duration, float targetVolume));
        //    Angry.Play(clipAngry);
        //}

    }

    private void MonsterChasing()
    {
        //if (true) 
        //{ 
        //StartCoroutine(FadeAudioSource.StartFade(AudioSource audioSource, float duration, float targetVolume));
        //Chasing.Play(clipChasing); 
        //}

    }

    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        //This function needs to be added when switching songs add a : StartCoroutine(FadeAudioSource.StartFade(AudioSource audioSource, float duration, float targetVolume));
        float currentTime = 0;
        float currentVolume;
        audioMixer.GetFloat(exposedParam, out currentVolume);
        currentVolume = Mathf.Pow(10, currentVolume / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);


        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVoloume = Mathf.Lerp(currentVolume, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVoloume) * 20);
            yield return null;
        }
        yield break;
    }
}
