using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SCR_Animated_Interactable_Multiplayer : NetworkBehaviour
{
    public enum LockState { Locked, Unlocked }
    [SerializeField] LockState lockState;
    public LockState lockStatus
    {
        get { return lockState; }
        private set { lockState = value; }
    }
    [SerializeField] Animator animator;


    NetworkVariable<bool> isOpened = new NetworkVariable<bool>();

    float openSpeed;
    bool canInteract;
    AudioSource SoundSource;
    [SerializeField] AudioClip SoundFX;
    [SerializeField] SCR_Key_Card_Reader keycardReader; // To revert change to "keyReader" 
    //[SerializeField] SCR_KeyReader keyReader; // Used to get SCR_KeyReader "Alexander" 


    bool isOpen;
    // lol
    private void Start()
    {
        if (animator == null)
            animator = GetComponentInParent<Animator>();

        SoundSource = GetComponent<AudioSource>();
        openSpeed = animator.speed;

        isOpened.Value = false;
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
            ChangeState();
        }
    }

    void ChangeState()
    {
        SetIsOpenedValueServerRpc(!isOpened.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    void SetIsOpenedValueServerRpc(bool newValue)
    {
        Debug.Log("i open door and play soud");
        isOpened.Value = newValue;

        animator.SetBool("isOpen", isOpened.Value);
        PlaySoundClientRpc();
    }

    public void MonsterOpenDoor()
    {
        if (isOpened.Value) return;

        ChangeState();
    }

    public void MonsterCloseDoor()
    {
        if (!isOpened.Value) return;

        ChangeState();
    }

    [ClientRpc]
    void PlaySoundClientRpc()
    {
        float randomPitch = Random.Range(0.9f, 1.1f);
        SoundSource.pitch = randomPitch;

        SoundSource.PlayOneShot(SoundFX);
    }
}
