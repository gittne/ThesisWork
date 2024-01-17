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
    [SerializeField] AudioClip reloadSound;
    [SerializeField] AudioSource audioSource;

    [Header("Rotation Angle")]
    XRGrabInteractable grabbable;

    [Header("Battery Variables")]
    [SerializeField] float batteryLife;
    [SerializeField] float minimumLightStrength;
    float maxBattery;

    void Start()
    {
        spotLight.enabled = false;
        lightBulb.enabled = false;

        grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(TurnOnOrOff);

        BatteryStrength();
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

    void BatteryStrength()
    {
        if (spotLight.enabled && batteryLife >= 0)
        {
            batteryLife -= Time.deltaTime;
        }

        spotLight.intensity = ((batteryLife + minimumLightStrength) / maxBattery);
        lightBulb.intensity = ((batteryLife + minimumLightStrength) / maxBattery);
    }

    public void RefillBatteries()
    {
        batteryLife = maxBattery;
        audioSource.PlayOneShot(reloadSound);
    }
}
