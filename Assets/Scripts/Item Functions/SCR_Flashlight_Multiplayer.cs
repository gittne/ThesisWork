using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Flashlight_Multiplayer : NetworkBehaviour
{
    [SerializeField] SCR_Inventory_Visual_Multiplayer inventory;

    [Header("Light Sources")]
    [SerializeField] Light spotLight;
    [SerializeField] Light lightBulb;

    [Header("Audio")]
    [SerializeField] AudioClip onSound;
    [SerializeField] AudioClip offSound;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] AudioSource audioSource;

    [Header("Battery Variables")]
    [SerializeField] float maxBattery;
    [SerializeField] float minimumLightStrength;
    NetworkVariable<float> batteryLife = new NetworkVariable<float>();

    bool isEnabled;

    void Start()
    {
        if (!IsOwner)
        {
            return;
        }

        spotLight.enabled = false;
        lightBulb.enabled = false;
        isEnabled = false;
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
            Debug.Log("battery life value: " + batteryLife.Value);

        if (Input.GetButtonDown("Fire1") && !inventory.isInventoryActive)
        {
            ToggleFlashlightServerRpc(isEnabled);

            isEnabled = !isEnabled;
        }

        BatteryStrength();
    }

    public void ChangeFlashlightState(bool currentState)
    {
        if (currentState)
        {
            audioSource.PlayOneShot(onSound);
            spotLight.enabled = true;
            lightBulb.enabled = true;
        }
        else
        {
            audioSource.PlayOneShot(offSound);
            spotLight.enabled = false;
            lightBulb.enabled = false;
        }
    }

    void BatteryStrength()
    {
        if (isEnabled && batteryLife.Value >= 0)
        {
            batteryLife.Value -= Time.deltaTime;
        }

        spotLight.intensity = ((batteryLife.Value + minimumLightStrength) / maxBattery);
        lightBulb.intensity = ((batteryLife.Value + minimumLightStrength) / maxBattery);
    }

    public void RefillBatteries()
    {
        batteryLife.Value = maxBattery;
        audioSource.PlayOneShot(reloadSound);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleFlashlightServerRpc(bool enabled)
    {
        ToggleFlashlightClientRpc(enabled);
    }

    [ClientRpc]
    public void ToggleFlashlightClientRpc(bool enabled)
    {
        ChangeFlashlightState(enabled);
    }
}
