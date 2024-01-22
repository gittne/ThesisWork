using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Written by Ben Schelhaas by the help of :https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/
public class SCR_Monster_Music : MonoBehaviour
{
    AudioSource[] musicSources;
    Coroutine soundSwitcherCoroutine;

    enum SoundState { NORMAL, HEIGHTENED, SEVERE }
    SoundState currentSoundState = SoundState.NORMAL;

    int soundStateIndex;

    public void SwitchToNormal()
    {
        if (currentSoundState == SoundState.NORMAL) return;

        PrepareMusicSwitch(0);
    }

    public void SwitchToHeightened()
    {
        if (currentSoundState == SoundState.HEIGHTENED) return;

        PrepareMusicSwitch(1);
    }

    public void SwitchToSevere()
    {
        if (currentSoundState == SoundState.SEVERE) return;

        PrepareMusicSwitch(2);
    }

    void PrepareMusicSwitch(int newSoundStateIndex)
    {
        if (soundSwitcherCoroutine != null)
        {
            StopCoroutine(soundSwitcherCoroutine);
            soundSwitcherCoroutine = null;

            for (int i = 0; i < musicSources.Length; i++)
            {
                if (i == soundStateIndex)
                    continue;

                musicSources[i].volume = 0;
            }
        }

        soundSwitcherCoroutine = StartCoroutine(SwitchSoundPlayer(newSoundStateIndex));
    }

    public IEnumerator SwitchSoundPlayer(int newSoundStateIndex)
    {
        AudioSource originalMusicSource = musicSources[soundStateIndex];
        AudioSource newMusicSource = musicSources[newSoundStateIndex];

        for(int i = 100; i > 0; i--)
        {
            originalMusicSource.volume = (float)i / 100;
            newMusicSource.volume = 1 - (float)i / 100;
            yield return new WaitForSeconds(0.01f);
        }

        soundStateIndex = newSoundStateIndex;
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
