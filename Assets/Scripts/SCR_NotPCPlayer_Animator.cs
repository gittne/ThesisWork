using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_NotPCPlayer_Animator : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void GetValues(bool running, float speed)
    {
        Debug.Log("Velocity is: " + speed);
        animator.SetBool("isRunning", running);
        animator.SetFloat("velocity", speed);
    }

    public void OtherPlayerDeath()
    {
        animator.SetTrigger("Death");
    }
}
