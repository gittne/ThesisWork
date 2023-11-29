using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementRedux : EnemyUtilities
{
    int roamRange = 50;

    EnemyState enemyState;

    GameObject player;

    NavMeshAgent agent;
    bool hasDestination;

    EnemyVision vision;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponentInChildren<EnemyVision>();
        enemyState = EnemyState.ROAM;
        if(GameObject.FindWithTag("Player") != null) {
            player = GameObject.FindWithTag("Player");
        }
    }

    private void Update()
    {
        if(enemyState == EnemyState.ROAM)
        {
            Roam();
        }
        else if (enemyState == EnemyState.FOLLOW)
        {
            Follow();
        }
        else if (enemyState != EnemyState.HUNT)
        {
            Hunt();
        }

        StatusUpdate();
        Debug.DrawLine(transform.position, agent.destination, Color.blue, 0.1f);

        if(vision.HasVisionOfPlayer)
        {
            EnemyUtilities utils = GetComponent<EnemyUtilities>();

            switch(utils.GetRage())
            {
                case < 50:
                    Debug.Log("not high enough");
                    break;
                case > 50:
                    Debug.Log("your death is imminent.");
                    break;
            }
        }
    }

    void Roam()
    {
        if (hasDestination)
            return;

        Debug.Log("Found new dstination.");
        agent.destination = RandomNavmeshPosition(roamRange);
        hasDestination = true;
    }

    void Follow()
    {

    }

    void Hunt()
    {

    }

    void StatusUpdate()
    {
        hasDestination = DestinationReach(transform.position, agent.destination);
    }
}
