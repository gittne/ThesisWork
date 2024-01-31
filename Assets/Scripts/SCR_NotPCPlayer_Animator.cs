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

    public void SendValues(bool running, bool moving)
    {
        animator.SetBool("isRunning", running);
        animator.SetBool("isMoving", moving);
        //animator.SetFloat("velocity", speed);
    }

    public void OtherPlayerDeath()
    {
        animator.SetTrigger("Death");
    }
}
