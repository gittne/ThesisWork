using System.Collections;
using System.Collections.Generic;
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
            // KILL
        }
    }
}
