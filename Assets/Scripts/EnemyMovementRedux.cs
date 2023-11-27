using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementRedux : EnemyUtilities
{
    int rageMeter;
    int roamRange;

    EnemyState enemyState;



    NavMeshAgent agent;
    bool hasDestination;

    private void Start()
    {
        enemyState = EnemyState.ROAM;
    }

    private void Update()
    {
        rageMeter = Mathf.Clamp(rageMeter, 0, 100);

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
    }

    void Roam()
    {
        if (hasDestination)
            return;

        RandomNavmeshPosition(roamRange);
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
