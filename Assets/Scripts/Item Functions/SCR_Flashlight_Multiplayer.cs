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
    [SerializeField] float batteryLife;
    [SerializeField] float minimumLightStrength;
    float maxBattery;

    bool isEnabled;

    NetworkVariable<float> BatteryLife;

    void Start()
    {
        if (!IsOwner)
        {
            return;
        }

        spotLight.enabled = false;
        lightBulb.enabled = false;
        isEnabled = false;
        maxBattery = batteryLife;
        BatteryLife.Value = batteryLife;
    }

    void Update()
    {
        if (!IsOwner)
        {
            if(BatteryLife != null)
                batteryLife = BatteryLife.Value;
        }

        if (IsOwner && Input.GetButtonDown("Fire1") && !inventory.isInventoryActive)
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
        if (isEnabled && batteryLife >= 0)
        {
            batteryLife -= Time.deltaTime;
            if(IsOwner) BatteryLife.Value = batteryLife;
        }

        spotLight.intensity = ((batteryLife + minimumLightStrength) / maxBattery);
        lightBulb.intensity = ((batteryLife + minimumLightStrength) / maxBattery);
    }

    public void RefillBatteries()
    {
        batteryLife = maxBattery;
        audioSource.PlayOneShot(reloadSound);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleFlashlightServerRpc(bool enabled, ServerRpcParams serverRpcParams = default)
    {
        ToggleFlashlightClientRpc(enabled);
    }

    [ClientRpc]
    public void ToggleFlashlightClientRpc(bool enabled)
    {
        ChangeFlashlightState(enabled);
    }
}
