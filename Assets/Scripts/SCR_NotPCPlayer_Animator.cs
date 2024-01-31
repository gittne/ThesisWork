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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleRunning(bool running)
    {
        animator.SetBool("isRunning", running);
    }
}
