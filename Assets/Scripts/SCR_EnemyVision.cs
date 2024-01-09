using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EnemyVision : MonoBehaviour
{
    SCR_EnemyBrain brain;
    RaycastHit hit;
    [SerializeField] LayerMask mask;
    bool hasVisionOfPlayer;

    public bool HasVisionOfPlayer { get { return hasVisionOfPlayer; } }

    [SerializeField] float visionLossDelay;
    float distanceToPlayer;

    GameObject visiblePlayer;

    // Start is called before the first frame update
    void Start()
    {
        brain = GetComponentInParent<SCR_EnemyBrain>();
        InvokeRepeating("RageTick", 0, 1);
    }

    private void Update()
    {
        visionLossDelay -= Time.deltaTime;
        visionLossDelay = Mathf.Clamp(visionLossDelay, 0, 3);

        if (visionLossDelay > 0)
            SendVisionInformation();
        else
        {
            hasVisionOfPlayer = false;

            if(brain.enemyState != SCR_EnemyUtilities.EnemyState.HUNT)
                brain.ReceiveVisionInformation(null);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (brain.enemyState != SCR_EnemyUtilities.EnemyState.ROAM && brain.enemyState != SCR_EnemyUtilities.EnemyState.FOLLOW && brain.enemyState != SCR_EnemyUtilities.EnemyState.HUNT)
            return;

        if (other.CompareTag("Player"))
        {
            Vector3 dir = -(transform.position - other.transform.position).normalized;
            Ray ray = new Ray(transform.position, other.transform.position.normalized);

            if(Physics.SphereCast(transform.position, 0.75f, dir, out hit, 50, mask))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    visiblePlayer = other.gameObject;
                    visionLossDelay = 2;
                    SendVisionInformation();
                    return;
                }
            }

            hasVisionOfPlayer = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasVisionOfPlayer = false;
        }
    }

    public void RageTick()
    {
        if (!hasVisionOfPlayer)
            return;

        if (distanceToPlayer > 10) distanceToPlayer = 10;
        int val = 15 - (int)Mathf.Round(distanceToPlayer);

        brain.AlterRage(val);
    }

    private void SendVisionInformation()
    {
        hasVisionOfPlayer = true;
        distanceToPlayer = Vector3.Distance(transform.position, hit.point);
        brain.ReceiveVisionInformation(visiblePlayer);
    }
}
