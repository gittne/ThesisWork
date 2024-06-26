using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MonsterAttractor : MonoBehaviour
{
    [SerializeField] private bool hasBeenInvestigated = true;

    public bool HasBeenInvestigated { get {  return hasBeenInvestigated; } }

    [ContextMenu("Activate squeaky toy")]
    public void BroadcastLocation()
    {
        hasBeenInvestigated = false;
    }

    public void Investigate()
    {
        hasBeenInvestigated = true;
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("KillZone") && !hasBeenInvestigated)
        {
            Debug.Log("I got investigated.");
            Investigate();
            other.gameObject.GetComponentInParent<SCR_EnemyBrain>().ResetMonster();
        }
    }
}
