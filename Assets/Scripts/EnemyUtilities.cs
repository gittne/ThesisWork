using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Holds a bunch of the methods to make the enemy movement script cleaner.

public class EnemyUtilities : MonoBehaviour
{
    public enum EnemyState { ROAM, FOLLOW, HUNT }
    public int[] rageSourceStrengths;

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
        if (Vector3.Distance(origin, target) < 0.5f)
            return true;
        return false;
    }
}
