using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SCR_Animated_Interactable : MonoBehaviour
{
    public enum LockState { Locked, Unlocked}
    [SerializeField] LockState lockState;
    public LockState lockStatus
    {
        get { return lockState; }
        private set { lockState = value; }
    }
    [SerializeField] Animator animator;

    
    float openSpeed;
    bool canInteract;
    AudioSource SoundSource;
    [SerializeField] AudioClip SoundFX;
    [SerializeField] SCR_Key_Card_Reader keyReader;

    
    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
        SoundSource = GetComponent<AudioSource>();
        openSpeed = animator.speed;
        canInteract = true;
    }

    private void Update()
    {
        if (keyReader != null)
        {
            if (keyReader.isActivated)
            {
                lockState = LockState.Unlocked;
            }
            else
            {
                lockState = LockState.Locked;
            }
        }
    }

    public void SwitchAnimationState()
    {
        if(canInteract && lockState == LockState.Unlocked)
        {
            StartCoroutine(ChangeState());
        }
    }

    IEnumerator ChangeState()
    {
        animator.SetTrigger("playAnim");
        
        canInteract = false;
        SoundSource.PlayOneShot(SoundFX);
        yield return new WaitForSeconds(openSpeed);
        
        
        canInteract = true;
    }
}
