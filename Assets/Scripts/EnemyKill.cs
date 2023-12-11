using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class EnemyKill : MonoBehaviour
{
    EnemyBrain brain;

    private void Start()
    {
        brain = GetComponentInParent<EnemyBrain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("I KILLED!");
            PlayerDeathServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void PlayerDeathServerRpc()
    {
        SCR_First_Person_Controller.PlayerDeathClientRpc();
    }
}
