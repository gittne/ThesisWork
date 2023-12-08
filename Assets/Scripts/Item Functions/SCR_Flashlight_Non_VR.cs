using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Flashlight_Non_VR : NetworkBehaviour
{
    [Header("Light Sources")]
    [SerializeField] Light spotLight;
    [SerializeField] Light lightBulb;

    [Header("Audio")]
    [SerializeField] AudioClip onSound;
    [SerializeField] AudioClip offSound;
    [SerializeField] AudioSource audioSource;

    public bool FlashLightIsOn { get; private set; }


    void Start()
    {
        if (!IsOwner)
        {
            return;
        }

        spotLight.enabled = false;
        lightBulb.enabled = false;
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            TurnOnOrOff();
        }
    }

    void TurnOnOrOff()
    {
        spotLight.enabled = !spotLight.enabled;
        lightBulb.enabled = !lightBulb.enabled;

        if (spotLight.enabled)
        {
            audioSource.PlayOneShot(onSound);
            FlashLightIsOn = true;
        }

        if (!spotLight.enabled)
        {
            audioSource.PlayOneShot(offSound);
            FlashLightIsOn = false;
        }
    }
}
