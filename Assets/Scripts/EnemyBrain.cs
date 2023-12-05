using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : EnemyUtilities
{
    int roamRange = 50;

    EnemyState enemyState;

    GameObject player;

    NavMeshAgent agent;
    bool hasDestination;

    EnemyVision vision;
    OLDENEMYSTUFF brain;

    [SerializeField] int rageMeter;
    [SerializeField] float rageDuration;
    bool rageLock;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponentInChildren<EnemyVision>();
        enemyState = EnemyState.ROAM;
        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    private void Update()
    {
        rageMeter = Mathf.Clamp(rageMeter, 0, 100);

        if (enemyState == EnemyState.ROAM) Roam();
        else Follow();

        StatusUpdate();
        Debug.DrawLine(transform.position, agent.destination, Color.blue, 0.1f);

        if (enemyState == EnemyState.HUNT)
        {
            rageDuration -= Time.deltaTime;
            if (vision.HasVisionOfPlayer)
            {
                rageDuration = 10;
            }

            if (rageDuration <= 0)
                enemyState = EnemyState.FOLLOW;
        }
    }

    void Roam()
    {
        if (hasDestination)
            return;

        agent.destination = RandomNavmeshPosition(roamRange);
        hasDestination = true;
    }

    void Follow()
    {

    }

    void StatusUpdate()
    {
        hasDestination = DestinationReach(transform.position, agent.destination);
    }

    public void AlterRage(int val)
    {
        rageMeter += val;
    }

    public void RageTick()
    {
        if (enemyState != EnemyState.HUNT) rageMeter--;

        if (rageMeter > 80) {
            enemyState = EnemyState.HUNT;
        }
        
    }
}
