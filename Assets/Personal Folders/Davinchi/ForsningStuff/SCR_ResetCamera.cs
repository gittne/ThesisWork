using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ResetCamera : MonoBehaviour
{
    public void AlertExit()
    {
        
        Debug.Log("Alerted end of exit");

        StartCoroutine(WaitForAnimation());
       
    }

    IEnumerator WaitForAnimation()
    {
        Animator animator = GetComponent<Animator>();
        yield return new WaitForSeconds(1.5f);
        Debug.Log("ExitDone");
        animator.enabled = false;
    }

}
