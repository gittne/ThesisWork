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
            other.gameObject.GetComponent<SCR_First_Person_Controller>().PlayerDeathServerRpc();
        }
    }
}
