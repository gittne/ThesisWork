using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElectricityChecker : MonoBehaviour
{
    EnemyMovement enemyMovement;
    NavMeshAgent agento;

    private void Start()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
        agento = GetComponentInParent<NavMeshAgent>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (enemyMovement.IsChasingElectricity)
            return;

        if (other.CompareTag("ElectricityEmitter"))
        {
            ElectricTrigger electricTrigger = other.GetComponent<ElectricTrigger>();

            if (electricTrigger.IsElectrified)
            {
                enemyMovement.GoChaseElectricity(other.transform);
            }
        }
    }
}
