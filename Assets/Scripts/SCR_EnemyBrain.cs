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

        if (Vector3.Distance(transform.position, currentTargetPlayer.transform.position) < 1) 
        { 
            FindNearestPlayer(); 
        }
    }

    public void CommenceRoam()
    {
        currentTargetPlayer = null;

        agent.speed = 3;
        agent.acceleration = 30;
        enemyState = EnemyState.ROAM;
        if(repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        hasDestination = false;
    }

    public void CommenceFollow(GameObject target)
    {
        hasDestination = false;

        agent.speed = 3.5f;
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
        if (enemyState == EnemyState.KILLING || enemyState == EnemyState.FINISHING)
            return;

        currentTargetPlayer = playa;

        if (currentTargetPlayer != null)
            if(rageMeter < 80) enemyState = EnemyState.FOLLOW;
        else enemyState = EnemyState.ROAM;
    }

    [ContextMenu("Teleport")]
    void GoFindMirror()
    {
        SCR_MirrorManager mirrorManager = FindObjectOfType<SCR_MirrorManager>();

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
        if(repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        agent.destination = transform.position;
    }

    [ContextMenu("Finish")]
    public void CommenceMultiplayerFinish()
    {
        if (!SCR_MultiplayerOverlord.Instance.IsHost) return;


        enemyState = EnemyState.KILLING;

        if (repositionCoroutine != null) StopCoroutine(repositionCoroutine);
        agent.destination = transform.position;

        agent.speed = 50;
        agent.angularSpeed = 1000;
        agent.velocity = agent.desiredVelocity;
        agent.acceleration = 500;

        GameObject[] players = new GameObject[2];
        players[0] = SCR_MultiplayerOverlord.Instance.Players[0].gameObject;
        players[1] = SCR_MultiplayerOverlord.Instance.Players[1].gameObject;

        if(Vector3.Distance(transform.position, players[0].transform.position) < Vector3.Distance(transform.position, players[1].transform.position))
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
        agent.destination = currentTargetPlayer.transform.position;
    }

    public void ResetMonster()
    {
        transform.position = new Vector3(0, 0, 0);
        enemyState = EnemyState.ROAM;
        rageMeter = 0;
    }
}