using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SCR_Key_Card_Reader : NetworkBehaviour
{
    public bool isActivated;
    public bool canReadCard { get; private set; }
    public bool canActivate;
    public string keycardItemID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canReadCard = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canReadCard = false;
        }
    }

    public void ReadCard()
    {
        if (SCR_MultiplayerOverlord.Instance == null)
        {
            isActivated = true;
        }
        else
        {
            ReadCardServerRpc();
        }
    }

    public void ActivateReader()
    {
        if(SCR_MultiplayerOverlord.Instance == null)
        {
            canActivate = true;
        }
        else
        {
            ReaderActivationServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ReaderActivationServerRpc()
    {
        ReaderActivationClientRpc();
    }

    [ClientRpc()]
    void ReaderActivationClientRpc()
    {
        canActivate = true;
    }

    [ServerRpc(RequireOwnership = false)]
    void ReadCardServerRpc()
    {
        ReadCardClientRpc();
    }

    [ClientRpc()]
    void ReadCardClientRpc()
    {
        isActivated = true;
    }
}
