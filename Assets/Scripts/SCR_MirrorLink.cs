using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MirrorLink : MonoBehaviour
{
    SCR_MirrorManager mirrorManager;

    private void Start()
    {
        mirrorManager = FindObjectOfType<SCR_MirrorManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SCR_EnemyBrain brain = other.gameObject.GetComponent<SCR_EnemyBrain>();

            if (!brain.CanTeleport) return;

            FindClosestExitToTarget(other.gameObject);
        }
           
    }

    public void FindClosestExitToTarget(GameObject enemy)
    {
        int chosenIndex = Random.Range(0, mirrorManager.Mirrors.Count - 1);

        while(chosenIndex == mirrorManager.LastEnteredMirror)
            chosenIndex = Random.Range(0, mirrorManager.Mirrors.Count - 1);

        enemy.GetComponent<SCR_EnemyBrain>().PerformMirrorWarp(mirrorManager.Mirrors[chosenIndex].transform.position);
    }
}
