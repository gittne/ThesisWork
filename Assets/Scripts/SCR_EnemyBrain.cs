using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class SCR_EnemyBrain : SCR_EnemyUtilities
{
    [SerializeField] TextMeshProUGUI enemyStateText;


    SCR_EnemyVision vision;

    int roamRange = 50;
    public EnemyState enemyState;

    NavMeshAgent agent;
    bool hasDestination;


    [SerializeField] int rageMeter;
    [SerializeField] float rageDuration;
    bool rageLock;
    bool repositionCooldown;

    GameObject currentTargetPlayer;

    float destinationReachScaleMeasure;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponentInChildren<SCR_EnemyVision>();
        enemyState = EnemyState.ROAM;

        destinationReachScaleMeasure = transform.localScale.y;
        InvokeRepeating("RageTick", 0, 1);
    }

    private void Update()
    {
        if (enemyState == EnemyState.ROAM)
        {
            enemyStateText.text = "ROAMING";
            enemyStateText.color = Color.blue;

            Roam();
        }
        else if (enemyState == EnemyState.FOLLOW)
        {
            enemyStateText.text = "FOLLLOWING";
            enemyStateText.color = Color.yellow;

            Follow();
        }
        else if (enemyState != EnemyState.HUNT)
        {
            enemyStateText.text = "HUNTING";
            enemyStateText.color = Color.red;

            Hunt();
        }

        StatusUpdate();
    }

    void Roam()
    {
        if (repositionCooldown || hasDestination)
            return;

        agent.destination = RandomNavmeshPosition(roamRange);
        hasDestination = true;
    }

    void Follow()
    {
        agent.destination = currentTargetPlayer.transform.position;
    }

    void Hunt()
    {
        agent.destination = currentTargetPlayer.transform.position;
    }

    public void CommenceRoam()
    {
        currentTargetPlayer = null;

        enemyState = EnemyState.ROAM;
    }

    public void CommenceFollow(GameObject target)
    {
        hasDestination = false;

        currentTargetPlayer = target;
        enemyState = EnemyState.FOLLOW;
    }

    public void CommenceHunt()
    {
        if (enemyState == EnemyState.HUNT)
            return;

        agent.speed = 5;
        enemyState = EnemyState.HUNT;
    }

    public void AlterRage(int val)
    {
        rageMeter += val;
    }

    public void RageTick()
    {
        if (enemyState != EnemyState.HUNT) rageMeter--;

        if (rageMeter > 80)
        {
            enemyState = EnemyState.HUNT;
        }
    }

    void StatusUpdate()
    {
        if (repositionCooldown)
            return;

        if (DestinationReach(transform.position, agent.destination, destinationReachScaleMeasure))
            StartCoroutine(RepositionDelay());
    }

    IEnumerator RepositionDelay()
    {
        repositionCooldown = true;
        yield return new WaitForSeconds(Random.Range(2, 5));

        repositionCooldown = false;
        hasDestination = false;
    }
}