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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("enemy entered my mirror");
            SCR_EnemyBrain brain = other.gameObject.GetComponent<SCR_EnemyBrain>();
            FindClosestExitToTarget(other.gameObject);
        }
           
    }

    public void FindClosestExitToTarget(GameObject enemy)
    {
        float lowestDistance = 9999;
        int chosenIndex = 0;

        Vector3 randomPlayerLocation = mirrorManager.Players[Random.Range(0, mirrorManager.Players.Count - 1)].transform.position;

        for (int i = 1; i < mirrorManager.Mirrors.Count; i++)
        {
            float currentMirrorDistance = Vector3.Distance(mirrorManager.Mirrors[i].transform.position, randomPlayerLocation);

            if(currentMirrorDistance < 10)
            {
                continue;
            }

            if (currentMirrorDistance < lowestDistance)
            {

                lowestDistance = currentMirrorDistance;
                chosenIndex = i;
            }
        }

        //enemy.transform.position = mirrorManager.Mirrors[chosenIndex].transform.position;
        enemy.GetComponent<SCR_EnemyBrain>().PerformMirrorWarp(mirrorManager.Mirrors[chosenIndex].transform.position);
    }
}
