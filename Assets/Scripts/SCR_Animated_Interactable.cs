using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SCR_Animated_Interactable : MonoBehaviour
{
    Animator animator;

    bool isOpen;
    float openSpeed;
    bool canInteract;

    private void Start()
    {
        animator = GetComponent<Animator>();
        openSpeed = animator.speed;
        canInteract = true;
    }

    public void SwitchAnimationState()
    {
        if(canInteract)
            StartCoroutine(ChangeState());
    }

    IEnumerator ChangeState()
    {
        animator.SetBool("Open", isOpen);
        canInteract = false;
        yield return new WaitForSeconds(openSpeed);

        isOpen = !isOpen;
        canInteract = true;
    }
}
