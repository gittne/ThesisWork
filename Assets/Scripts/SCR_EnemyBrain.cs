using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Rendering;

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

    GameObject currentTargetPlayer;

    float destinationReachScaleMeasure;

    Coroutine repositionCoroutine;

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
        rageMeter = Mathf.Clamp(rageMeter, 0, 100);

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

        if (enemyState == EnemyState.HUNT) HuntFumes();
    }

    void Roam()
    {
        if (hasDestination)
            return;

        if(repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        repositionCoroutine = StartCoroutine(RepositionDelay());
    }

    void Follow()
    {
        agent.destination = currentTargetPlayer.transform.position;
        Debug.DrawLine(transform.position, currentTargetPlayer.transform.position, Color.black, 0.1f);
    }

    void Hunt()
    {
        agent.destination = currentTargetPlayer.transform.position;
    }

    public void CommenceRoam()
    {
        currentTargetPlayer = null;

        agent.speed = 3;
        enemyState = EnemyState.ROAM;
    }

    public void CommenceFollow(GameObject target)
    {
        hasDestination = false;

        currentTargetPlayer = target;
        enemyState = EnemyState.FOLLOW;
    }

    [ContextMenu("Hunt")]
    public void CommenceHunt()
    {
        if (enemyState == EnemyState.HUNT)
            return;


        currentTargetPlayer = FindNearestPlayer();
        Debug.DrawLine(transform.position, currentTargetPlayer.transform.position, Color.white, 10);
        agent.speed = 100;
        rageDuration = 10;
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
            CommenceHunt();
        }
    }

    IEnumerator RepositionDelay()
    {
        agent.destination = RandomNavmeshPosition(roamRange);
        hasDestination = true;

        while (DestinationReach(transform.position, agent.destination, destinationReachScaleMeasure))
            yield return null;

        Debug.Log("I hav e reached my position, time to rest.");
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        Debug.Log("I dont have a destination");
        hasDestination = false;
    }

    void HuntFumes()
    {
        rageDuration -= Time.deltaTime;
        if (vision.HasVisionOfPlayer)
        {
            rageDuration = 10;
        }

        if (rageDuration <= 0)
        {
            rageMeter = 0;
            CommenceRoam();
        }
    }
}