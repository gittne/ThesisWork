using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//Holds a bunch of the methods to make the enemy movement script cleaner.

public class SCR_EnemyUtilities : MonoBehaviour
{
    public enum EnemyState { ROAM, FOLLOW, HUNT, TELEPORTING }

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
    public bool DestinationReach(Vector3 origin, Vector3 target, float range)
    {
        Vector3 originFoot = origin -= new Vector3(0, 1, 0);


        if (Vector3.Distance(originFoot, target) < range + 0.5f)
        {
            return false;
        }
  
        return true;
    }

    public GameObject FindNearestPlayer()
    {
        List<GameObject> players = new List<GameObject>();
        List<float> distances = new List<float>();  

        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(p);
            distances.Add(Vector3.Distance(transform.position, p.transform.position));
        }

        if(players.Count == 1) return players[0];

        if (distances[0] < distances[1]) return players[0];
        else return players[1];
    }
}
