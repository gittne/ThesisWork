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
            KillPlayers(other.gameObject);
        }
    }

    void KillPlayers(GameObject player)
    {
        if (SCR_MultiplayerOverlord.Instance != null)
        {
            Debug.Log("multiplayer kill.");
            SCR_MultiplayerOverlord.Instance.KillAllPlayersServerRpc();
        }
        else
        {
            Debug.Log("singleplayer kill.");
            //player.transform.position = new Vector3(0, 0, 0);
        }
    }
}
