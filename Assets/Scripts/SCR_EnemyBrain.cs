using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCR_EnemyBrain : SCR_EnemyUtilities
{
    int roamRange = 50;

    public EnemyState enemyState;

    GameObject player;

    NavMeshAgent agent;
    bool hasDestination;

    EnemyVision vision;

    [SerializeField] int rageMeter;
    [SerializeField] float rageDuration;
    bool rageLock;

    float destinationReachScaleMeasure;

    Coroutine NewPosition;
    bool repositionCooldown;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponentInChildren<EnemyVision>();
        enemyState = EnemyState.ROAM;
        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player");
        }

        destinationReachScaleMeasure = transform.localScale.y;

        InvokeRepeating("RageTick", 0, 1);
    }

    private void Update()
    {
        rageMeter = Mathf.Clamp(rageMeter, 0, 100);
        StatusUpdate();

        if (enemyState == EnemyState.HUNT)
        {
            rageDuration -= Time.deltaTime;
            if (vision.HasVisionOfPlayer)
            {
                rageDuration = 10;
            }

            if (rageDuration <= 0)
            {
                enemyState = EnemyState.FOLLOW;
                agent.speed = 3;
            }
        }

        if (enemyState == EnemyState.ROAM && !repositionCooldown) Roam();
        else Follow();

        Debug.DrawLine(transform.position, agent.destination, Color.blue, 0.1f);
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
        if (repositionCooldown)
            return;

        if (DestinationReach(transform.position, agent.destination, destinationReachScaleMeasure))
            StartCoroutine(RepositionDelay());
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
            agent.speed = 6;
        }
    }

    public void SetTargetPlayer(GameObject target)
    {
        agent.destination = target.transform.position;
    }

    public void KilledPlayer()
    {
        hasDestination = false;
        rageMeter = 0;
        rageDuration = 0;
        enemyState = EnemyState.ROAM;
    }

    IEnumerator RepositionDelay()
    {
        repositionCooldown = true;
        yield return new WaitForSeconds(Random.Range(2, 5));

        repositionCooldown = false;
        hasDestination = false;
    }
}
