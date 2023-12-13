using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Flashlight_Non_VR : MonoBehaviour
{
    [SerializeField] SCR_First_Person_Controller_Singleplayer controllerScript;

    [Header("Light Sources")]
    [SerializeField] Light spotLight;
    [SerializeField] Light lightBulb;

    [Header("Audio")]
    [SerializeField] AudioClip onSound;
    [SerializeField] AudioClip offSound;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        spotLight.enabled = false;
        lightBulb.enabled = false;
    }

    void Update()
    {
        if (!controllerScript.isInventoryActive)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                TurnOnOrOff();
            }
        }
        else
        {
            spotLight.enabled = false;
            lightBulb.enabled = false;
        }
    }

    void TurnOnOrOff()
    {
        spotLight.enabled = !spotLight.enabled;
        lightBulb.enabled = !lightBulb.enabled;

        if (spotLight.enabled)
        {
            audioSource.PlayOneShot(onSound);
        }

        if (!spotLight.enabled)
        {
            audioSource.PlayOneShot(offSound);
        }
    }
}
