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
        if (other.CompareTag("Player"))
        {
            Debug.Log("I KILLED!");
            PlayerDeathServerRpc(other.gameObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void PlayerDeathServerRpc(GameObject other)
    {
        other.gameObject.GetComponent<SCR_First_Person_Controller>().PlayerDeathClientRpc();
    }
}
