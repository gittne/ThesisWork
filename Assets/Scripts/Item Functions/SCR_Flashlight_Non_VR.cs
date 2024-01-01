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

    [Header("Battery Variables")]
    [SerializeField] float batteryLife;
    [SerializeField] float minimumLightStrength;
    float maxBattery;

    void Start()
    {
        spotLight.enabled = false;
        lightBulb.enabled = false;
        maxBattery = batteryLife;
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

        BatteryStrength();
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

    void BatteryStrength()
    {
        if (spotLight.enabled && batteryLife >= 0)
        {
            batteryLife -= Time.deltaTime;
        }

        spotLight.intensity = (batteryLife / maxBattery) + minimumLightStrength;
        lightBulb.intensity = (batteryLife / maxBattery) + minimumLightStrength;
    }

    public void RefillBatteries()
    {
        batteryLife = maxBattery;
    }
}
