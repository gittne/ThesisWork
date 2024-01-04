using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCR_EnemyAnimator : MonoBehaviour
{
    SCR_EnemyBrain brain;
    Animator animator;
    NavMeshAgent agent;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponent<SCR_EnemyBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isHunting", brain.enemyState == SCR_EnemyUtilities.EnemyState.HUNT ?  true : false);
        speed = agent.velocity.magnitude;

        animator.SetFloat("speed", speed);
    }

    public void PlayKillAnimation()
    {
        StartCoroutine(KillAnimationCoroutine());
    }

    IEnumerator KillAnimationCoroutine()
    {
        animator.SetBool("kill", true);
        yield return new WaitForSeconds(4);
        animator.SetBool("kill", false);
    }
}
