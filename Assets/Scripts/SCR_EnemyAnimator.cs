using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCR_EnemyAnimator : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = agent.velocity.magnitude;

        animator.SetFloat("speed", speed);
    }
}
