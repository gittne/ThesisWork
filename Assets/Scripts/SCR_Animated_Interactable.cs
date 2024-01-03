using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SCR_Animated_Interactable : MonoBehaviour
{
    enum LockState { Locked, Unlocked}
    [SerializeField] LockState lockState;
    Animator animator;

    bool isOpen;
    float openSpeed;
    bool canInteract;
    AudioSource SoundSource;
    [SerializeField] AudioClip SoundFX;
    [SerializeField] SCR_Key_Card_Reader keyReader;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SoundSource = GetComponent<AudioSource>();
        openSpeed = animator.speed;
        canInteract = true;
        if (keyReader == null)
        {
            return;
        }
    }

    public void SwitchAnimationState()
    {
        if (keyReader.isActivated)
        {
            lockState = LockState.Unlocked;
        }
        else
        {
            lockState = LockState.Locked;
        }

        if(canInteract && lockState == LockState.Unlocked)
            StartCoroutine(ChangeState());
    }

    IEnumerator ChangeState()
    {
        animator.SetBool("Open", isOpen);
        canInteract = false;
        SoundSource.PlayOneShot(SoundFX);
        yield return new WaitForSeconds(openSpeed);

        isOpen = !isOpen;
        canInteract = true;
    }
}
