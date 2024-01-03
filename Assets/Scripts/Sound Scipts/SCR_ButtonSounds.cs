using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ButtonSounds : MonoBehaviour
{
     AudioSource sourceButton;
    [SerializeField] AudioClip buttonClip;

    public void playButton ()
    {
        sourceButton.PlayOneShot(buttonClip);
    }
}
