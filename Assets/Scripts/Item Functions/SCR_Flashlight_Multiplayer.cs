using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Flashlight_Multiplayer : NetworkBehaviour
{
    [Header("Light Sources")]
    [SerializeField] Light spotLight;
    [SerializeField] Light lightBulb;

    [Header("Audio")]
    [SerializeField] AudioClip onSound;
    [SerializeField] AudioClip offSound;
    [SerializeField] AudioSource audioSource;

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

        if (Input.GetButtonDown("Fire1"))
        {
            ToggleFlashlightClientRpc(isEnabled);
        }
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

    [ClientRpc]
    public void ToggleFlashlightClientRpc(bool enabled, ServerRpcParams serverRpcParams = default)
    {
        ChangeFlashlightState(enabled);

        isEnabled = !isEnabled;
    }
}
