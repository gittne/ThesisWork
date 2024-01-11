using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MonsterAttractor : MonoBehaviour
{
    private bool hasBeenInvestigated = true;

    public bool HasBeenInvestigated { get {  return hasBeenInvestigated; } }

    public void BroadcastLocation()
    {
        hasBeenInvestigated = false;
    }

    public void Investigate()
    {
        hasBeenInvestigated = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Investigate();
            other.gameObject.GetComponent<SCR_EnemyBrain>().CommenceRoam();
        }
    }
}
