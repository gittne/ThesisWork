using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SCR_Animated_Interactable : MonoBehaviour
{
    public enum LockState { Locked, Unlocked }
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
    [SerializeField] SCR_Key_Card_Reader keycardReader; // To revert change to "keyReader" 
    //[SerializeField] SCR_KeyReader keyReader; // Used to get SCR_KeyReader "Alexander" 


    bool isOpen;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        SoundSource = GetComponent<AudioSource>();
        openSpeed = animator.speed;
    }

    private void Update()
    {
        if (keycardReader != null)
        {
            if (keycardReader.isActivated)
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

        if (lockState == LockState.Unlocked)
        {
            Debug.Log("toggling my open / closed state.");

            ChangeState();
        }
        else
        {
            Debug.Log("I am a locked door.");
        }
    }

    void ChangeState()
    {
        isOpen = !isOpen;

        animator.SetBool("isOpen", isOpen);
        SoundSource.PlayOneShot(SoundFX);
    }

    public void MonsterOpenDoor()
    {
        if (isOpen) return;

        ChangeState();
    }

    public void MonsterCloseDoor()
    {
        if (!isOpen) return;

        ChangeState();
    }
}
