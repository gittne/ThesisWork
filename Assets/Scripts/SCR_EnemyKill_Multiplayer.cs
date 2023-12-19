using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SCR_EnemyKill_Multiplayer : MonoBehaviour
{
    SCR_EnemyBrain brain;

    private void Start()
    {
        brain = GetComponentInParent<SCR_EnemyBrain>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SCR_MultiplayerOverlord.Instance.KillAllPlayersServerRpc();
        }
    }
}
