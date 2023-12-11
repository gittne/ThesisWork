using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//Holds a bunch of the methods to make the enemy movement script cleaner.

public class EnemyUtilities : MonoBehaviour
{
    public enum EnemyState { ROAM, FOLLOW, HUNT }

    //Get a random position on a navmesh sphere around it.
    public Vector3 RandomNavmeshPosition(float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    //Checks if it is close enough to its roam destination to find a new one.
    public bool DestinationReach(Vector3 origin, Vector3 target)
    {
        Vector3 originFoot = origin -= new Vector3(0, 1, 0);


        if (Vector3.Distance(originFoot, target) < 1f)
        {
            return false;
        }
  
        return true;
    }
}
