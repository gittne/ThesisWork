using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Rendering;

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

    [SerializeField] Transform monsterResetPosition;

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
            HuntFumes();
        }
        else if (enemyState == EnemyState.FINISHING)
        {
            Finishing();
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
            agent.destination = currentTargetPlayer.transform.position;
    }

    void Hunt()
    {
        agent.destination = currentTargetPlayer.transform.position;
    }

    public void CommenceRoam()
    {
        currentTargetPlayer = null;

        agent.speed = 2.5f;
        agent.acceleration = 15;
        enemyState = EnemyState.ROAM;
        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        hasDestination = false;
    }

    public void CommenceFollow(GameObject target)
    {
        hasDestination = false;

        agent.speed = 3f;
        agent.acceleration = 30;
        currentTargetPlayer = target;
        enemyState = EnemyState.FOLLOW;
        if (repositionCoroutine != null && currentTargetPlayer == null) StopCoroutine(repositionCoroutine);
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

    public void RageTick()
    {
        if (enemyState == EnemyState.TELEPORTING || enemyState == EnemyState.KILLING) return;


        if (enemyState != EnemyState.HUNT) rageMeter--;

        if (rageMeter > 80) CommenceHunt();
    }

    IEnumerator RepositionDelay()
    {
        int mirrorChance = Random.Range(0, 10);
        if (mirrorChance >= 8)
        {
            GoFindMirror();
            yield break;
        }

        agent.destination = RandomNavmeshPosition(roamRange);

        hasDestination = true;

        while (DestinationReach(transform.position, agent.destination, destinationReachScaleMeasure))
            yield return null;

        yield return new WaitForSeconds(Random.Range(1.5f, 3f));

        hasDestination = false;
    }

    void HuntFumes()
    {
        rageDuration -= Time.deltaTime;

        if (vision.HasVisionOfPlayer)
        {
            rageDuration = 7;
        }

        if (rageDuration <= 0)
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

        agent.speed = 4.5f;

        wantsToTeleport = true;
        enemyState = EnemyState.TELEPORTING;
        agent.destination = mirrorManager.FindClosestEntrance(transform.position);
        Debug.Log("heading for mirror");
    }

    public void PerformMirrorWarp(Vector3 dest)
    {
        if (!wantsToTeleport)
        {
            if (enemyState != EnemyState.FOLLOW || enemyState != EnemyState.HUNT) CommenceRoam();
            return;
        }

        agent.Warp(dest);
        wantsToTeleport = false;
        CommenceRoam();
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
        players[0] = SCR_MultiplayerOverlord.Instance.Players[0].gameObject;
        players[1] = SCR_MultiplayerOverlord.Instance.Players[1].gameObject;

        if (players[0].GetComponent<SCR_First_Person_Controller>().IsDead)
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
        hasDestination = false;
        transform.position = monsterResetPosition.position;
        enemyState = EnemyState.ROAM;
        rageMeter = 0;
        Debug.Log("im reset");
    }

    public void CommenceInvestigation(GameObject target)
    {
        enemyState = EnemyState.INVESTIGATING;
        agent.speed = 3f;
        agent.acceleration = 30;

        agent.destination = target.transform.position;
        currentTargetPlayer = target;
    }
}