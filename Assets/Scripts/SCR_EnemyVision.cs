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

    float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        brain = GetComponentInParent<SCR_EnemyBrain>();
        InvokeRepeating("RageTick", 0, 1);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 dir = -(transform.position - other.transform.position).normalized;
            Ray ray = new Ray(transform.position, other.transform.position.normalized);

            if(Physics.SphereCast(transform.position, 0.75f, dir, out hit, 30, mask))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Debug.DrawRay(transform.position, 10 * dir, UnityEngine.Color.red, 2);

                    hasVisionOfPlayer = true;
                    distanceToPlayer = Vector3.Distance(transform.position, hit.point);
                    brain.SetTargetPlayer(other.gameObject);
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
}
