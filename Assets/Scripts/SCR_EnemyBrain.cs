using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System.Net;

public class SCR_EnemyBrain : SCR_EnemyUtilities
{
    SCR_EnemyVision vision;
    SCR_EnemyAnimator animator;

    int roamRange = 20;
    public EnemyState enemyState;

    NavMeshAgent agent;
    bool hasDestination;


    [SerializeField] int rageMeter;
    [SerializeField] float rageDuration;
    bool rageLock;

    GameObject currentTargetPlayer;

    float destinationReachScaleMeasure;

    Coroutine repositionCoroutine;

    bool wantsToTeleport;
    bool canTeleport = true;
    public bool CanTeleport { get { return canTeleport; } set { canTeleport = value; } }

    float investigationInterestDuration;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<SCR_EnemyAnimator>();
        vision = GetComponentInChildren<SCR_EnemyVision>();
        enemyState = EnemyState.ROAM;

        destinationReachScaleMeasure = transform.localScale.y;
        InvokeRepeating("RageTick", 0, 1);
    }

    private void Update()
    {
        rageMeter = Mathf.Clamp(rageMeter, 0, 100);

        agent.velocity = agent.desiredVelocity;

        if (enemyState == EnemyState.ROAM)
        {
            Roam();
        }
        else if (enemyState == EnemyState.FOLLOW)
        {
            Follow();
        }
        else if (enemyState == EnemyState.HUNT)
        {
            Hunt();
        }
        else if (enemyState == EnemyState.FINISHING)
        {
            Finishing();
        }
        else if (enemyState == EnemyState.INVESTIGATING)
        {
            Investigating();
        }
    }

    void Roam()
    {
        if (hasDestination)
            return;

        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        repositionCoroutine = StartCoroutine(RepositionDelay());
    }

    void Follow()
    {
        if (currentTargetPlayer != null)
        {
            agent.destination = currentTargetPlayer.transform.position;
            return;
        }

        if (hasDestination)
            return;

        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        repositionCoroutine = StartCoroutine(RepositionDelay());
    }

    void Hunt()
    {
        agent.destination = currentTargetPlayer.transform.position;
    }

    public void CommenceRoam()
    {
        currentTargetPlayer = null;

        agent.speed = 2.5f;
        agent.acceleration = 30;
        enemyState = EnemyState.ROAM;
        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        hasDestination = false;
    }

    public void CommenceFollow(GameObject target)
    {
        enemyState = EnemyState.FOLLOW;

        if(rageMeter < 20)
            rageMeter = 30;
        if (repositionCoroutine != null && currentTargetPlayer == null) StopCoroutine(repositionCoroutine);
        hasDestination = false;

        agent.speed = 3f;
        agent.acceleration = 30;
        currentTargetPlayer = target;
        agent.destination = target.transform.position;
    }

    [ContextMenu("Hunt")]
    public void CommenceHunt()
    {
        currentTargetPlayer = FindNearestPlayer();
        agent.speed = 7;
        agent.velocity = agent.desiredVelocity;

        rageDuration = 7;

        agent.acceleration = 100;
        enemyState = EnemyState.HUNT;
        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);
    }

    public void AlterRage(int val)
    {
        rageMeter += val;
    }

    public int GetRageAmount()
    {
        return rageMeter;
    }

    public void RageTick()
    {
        if (enemyState == EnemyState.KILLING || enemyState == EnemyState.FINISHING || enemyState == EnemyState.TELEPORTING) return;

        RageDurationTick();

        if (enemyState != EnemyState.HUNT) rageMeter--;

        if (rageMeter > 80 && enemyState != EnemyState.HUNT) CommenceHunt();

        if (rageMeter < 20 && enemyState != EnemyState.ROAM && enemyState != EnemyState.INVESTIGATING) CommenceRoam();
    }

    IEnumerator RepositionDelay()
    {
        int mirrorChance = Random.Range(0, 10);
        if (mirrorChance >= 8 && canTeleport)
        {
            if (Vector3.Distance(transform.position, FindNearestPlayer().transform.position) > 15)
            {
                hasDestination = true;
                GoFindMirror();
                yield break;
            }
        }

        agent.destination = RandomNavmeshPosition(roamRange);

        hasDestination = true;

        while (DestinationReach(transform.position, agent.destination, destinationReachScaleMeasure))
            yield return null;

        yield return new WaitForSeconds(Random.Range(1.5f, 3f));

        hasDestination = false;
    }

    void RageDurationTick()
    {
        rageDuration -= Time.deltaTime;
        rageDuration = Mathf.Clamp(rageDuration, 0, 8);

        if (vision.HasVisionOfPlayer)
        {
            rageDuration = 7;
        }

        if (rageDuration <= 0 && enemyState == EnemyState.HUNT)
        {
            rageMeter = 0;
            CommenceRoam();
        }
    }

    public void ReceiveVisionInformation(GameObject playa)
    {
        if (enemyState == EnemyState.KILLING)
            return;

        currentTargetPlayer = playa;

        if (currentTargetPlayer != null)
            if (rageMeter < 80) enemyState = EnemyState.FOLLOW;
    }

    [ContextMenu("Teleport")]
    void GoFindMirror()
    {

        SCR_MirrorManager mirrorManager = FindObjectOfType<SCR_MirrorManager>();
        if (mirrorManager == null) return;

        if (mirrorManager.Mirrors.Count == 0) return;

        wantsToTeleport = true;
        enemyState = EnemyState.TELEPORTING;

        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);

        agent.speed = 4.5f;
        agent.destination = mirrorManager.FindClosestEntrance(transform.position);
    }

    public void PerformMirrorWarp(Vector3 dest)
    {
        if (!canTeleport)
                return;

        wantsToTeleport = false;

        StartCoroutine(SetMirroringOnCooldown());
        agent.Warp(dest);

        CommenceInvestigation(FindNearestPlayer());
        Debug.DrawLine(transform.position, FindNearestPlayer().transform.position, Color.red, 10);
    }

    public void EnterKillingState()
    {
        enemyState = EnemyState.KILLING;
        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        agent.destination = transform.position;
    }

    [ContextMenu("Finish")]
    public void CommenceMultiplayerFinish()
    {
        if (!SCR_MultiplayerOverlord.Instance.IsHost) return;


        enemyState = EnemyState.KILLING;

        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        agent.destination = transform.position;

        agent.speed = 15;
        agent.angularSpeed = 1000;
        agent.velocity = agent.desiredVelocity;
        agent.acceleration = 500;

        GameObject[] players = new GameObject[2];
        players[0] = SCR_MultiplayerOverlord.Instance.PlayerObjects[0].gameObject;
        players[1] = SCR_MultiplayerOverlord.Instance.PlayerObjects[1].gameObject;

        if (players[0].GetComponent<SCR_First_Person_Controller>().AmIDead.Value)
        {
            currentTargetPlayer = players[1];
        }
        else
        {
            currentTargetPlayer = players[0];
        }

        enemyState = EnemyState.FINISHING;
    }

    void Finishing()
    {
        rageMeter = 100;

        if (currentTargetPlayer == null)
        {
            agent.destination = GameObject.FindWithTag("Player").transform.position;
            return;
        }
        agent.destination = currentTargetPlayer.transform.position;
    }

    public void ResetMonster()
    {
        CommenceRoam();
        rageMeter = 0;
        Debug.Log("im reset");
    }

    public void CommenceInvestigation(GameObject target)
    {
        enemyState = EnemyState.INVESTIGATING;
        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);

        agent.speed = 5f;
        agent.acceleration = 50;

        investigationInterestDuration = 5;

        agent.destination = target.transform.position;
        currentTargetPlayer = target;
    }

    void Investigating()
    {
        investigationInterestDuration -= Time.deltaTime;
        investigationInterestDuration = Mathf.Clamp(investigationInterestDuration, 0, 5);

        if (investigationInterestDuration <= 0)
        {
            if (rageMeter < 20)
                CommenceRoam();
            else if (rageMeter < 80 && rageMeter > 20)
                CommenceFollow(FindNearestPlayer());
            else
                CommenceRoam();
        }
    }
    
    IEnumerator SetMirroringOnCooldown()
    {
        canTeleport = false;
        yield return new WaitForSeconds(10);
        canTeleport = true;
    }
}