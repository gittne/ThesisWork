using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_Flashlight : MonoBehaviour
{
    [Header("Light Sources")]
    [SerializeField] Light spotLight;
    [SerializeField] Light lightBulb;

    [Header("Controller Input Binding")]
    [SerializeField] InputActionProperty flipLightButton;

    [Header("Audio")]
    [SerializeField] AudioClip onSound;
    [SerializeField] AudioClip offSound;
    [SerializeField] AudioSource audioSource;

    [Header("Rotation Angle")]
    [SerializeField] Vector3 rotationAngles;
    XRGrabInteractable grabbable;

    void Start()
    {
        spotLight.enabled = false;
        lightBulb.enabled = false;

        grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(TurnOnOrOff);
        
    }

    void TurnOnOrOff(ActivateEventArgs arg)
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
