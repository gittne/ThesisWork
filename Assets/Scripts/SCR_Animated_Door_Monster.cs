using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Animated_Door_Monster : MonoBehaviour
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
    [SerializeField] SCR_Key_Card_Reader keycardReader;
    [SerializeField] float closeThreshold;
    HingeJoint hinge;

    bool isOpen;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        SoundSource = GetComponent<AudioSource>();
        openSpeed = animator.speed;

        animator.enabled = false;

        hinge = GetComponent<HingeJoint>();

        isOpen = false;
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

        if (hinge.axis.z > closeThreshold)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            animator.enabled = true;
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            animator.enabled = false;
        }
        else
        {
            Debug.Log("Wrong tag");
            return;
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
