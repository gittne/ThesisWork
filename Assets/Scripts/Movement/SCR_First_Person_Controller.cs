using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using VivoxUnity;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.Netcode.Components;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class SCR_First_Person_Controller : NetworkBehaviour
{
    //SUMMARY: This script is responsible for character movement and looking
    //Base code provided by "Comp-3 Interactive": https://www.youtube.com/watch?v=Ew4l5RPltG8&list=PLfhbBaEcybmgidDH3RX_qzFM0mIxWJa21

    public bool canMove { get; private set; } = true;
    public float crouchTimer { get; private set; }
    public bool isRunning => canSprintDebug && Input.GetKey(sprintKey);
    public bool shouldCrouch => !duringCrouchAnimation && characterController.isGrounded && Input.GetKeyDown(crouchKey) 
        && !Physics.Raycast(characterController.transform.position, characterController.transform.up, out crouchRaycast, (characterController.height / 2) + crouchRaycastModifier);

    [SerializeField] GameObject playerCamera;
    [SerializeField] Transform cameraHolder;
    [SerializeField] CharacterController characterController;
    [Header("Functions")]
    [SerializeField] Vector3 spawnpoint;
    [SerializeField] bool canSprintDebug = true;
    [SerializeField] bool canCrouchDebug = true;
    [SerializeField] bool canHeadbobDebug = true;

    [Header("Controls")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode radioKey = KeyCode.Q;

    [Header("Movement Variables")]
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;
    [SerializeField] float crouchSpeed = 1f;
    [SerializeField] float gravity = 30f;

    [Header("Inventory Variables")]
    [SerializeField] SCR_Inventory_Visual_Multiplayer visualInventory;

    [Header("Mouse Look Variables")]
    [SerializeField, Range(1, 10)] float xLookSensitivity = 2f;
    [SerializeField, Range(1, 10)] float yLookSensitivity = 2f;
    [SerializeField, Range(1, 100)] float upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] float lowerLookLimit = 80f;

    [Header("Crouching Variables")]
    [SerializeField] float crouchingHeight;
    public float crouchHeight
    {
        get { return crouchingHeight; }
        private set { crouchingHeight = value; }
    }
    [SerializeField] float standingHeight;
    public float standHeight
    {
        get { return standingHeight; }
        private set { standingHeight = value; }
    }
    [SerializeField] float timeToCrouch;
    public float crouchTime
    {
        get { return timeToCrouch; }
        private set { timeToCrouch = value; }
    }
    [SerializeField] Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] Vector3 standingCenter = new Vector3(0, 0, 0);
    bool isCrouching;
    bool duringCrouchAnimation;
    RaycastHit crouchRaycast;
    float crouchRaycastModifier;

    [Header("Headbob Variables")]
    [SerializeField] float walkBobSpeed;
    [SerializeField] float walkBobAmount;
    [SerializeField] float runningBobSpeed;
    [SerializeField] float runningBobAmount;
    [SerializeField] float crouchBobSpeed;
    [SerializeField] float crouchBobAmount;
    [SerializeField] float yAxisMultiplier;
    float yDefaultPosition = 0;
    float headbobTimer;

    Vector3 movementDirection;
    Vector2 currentInput;

    float xRotation = 0;

    Image fader;

    SCR_MultiplayerOverlord overlord;
    NuisanceEmitter nuisance;

    public NetworkVariable<bool> AmIDead;

    Vector3 respawnLocation;

    void Start()
    {
        if (!IsOwner) return;

        Debug.Log("im the owner of this one");
        StartCoroutine(SetupDelay());
        fader = GameObject.FindGameObjectWithTag("BlackFade").GetComponent<Image>();
        nuisance = GetComponentInChildren<NuisanceEmitter>();

        spawnpoint = GameObject.FindWithTag("RespawnLocation").transform.position;
        ClientNetworkTransform cnt = GetComponent<ClientNetworkTransform>();
        cnt.Teleport(spawnpoint, Quaternion.identity, transform.localScale);
    }

    public override void OnNetworkSpawn()
    {
        AmIDead.Value = false;
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        visualInventory.InventoryManagement();

        if (canMove && visualInventory.isInventoryActive == false)
        {
            MovementInput();

            MouseLook();

            ApplyMovement();

            if (canCrouchDebug)
            {
                Crouch();
            }

            if (canHeadbobDebug)
            {
                Headbob();
            }
        }

        if (overlord == null)
            return;

        if(overlord.MonsterBrain != null)
        {
            if (overlord.MonsterBrain.enemyState != SCR_EnemyUtilities.EnemyState.HUNT &&
            overlord.MonsterBrain.enemyState != SCR_EnemyUtilities.EnemyState.KILLING)
            {
                if (Input.GetKeyDown(radioKey)) EnableRadio();
            }

            if (Input.GetKeyUp(radioKey) ||
                overlord.MonsterBrain.enemyState == SCR_EnemyUtilities.EnemyState.HUNT ||
                overlord.MonsterBrain.enemyState == SCR_EnemyUtilities.EnemyState.KILLING)
                    DisableRadio();
        }
    }

    void MovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : isRunning? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical"),
            (isCrouching ? crouchSpeed : isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal"));

        float movementDirectionY = movementDirection.y;

        movementDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);

        movementDirection.y = movementDirectionY;
    }

    void MouseLook()
    {
        xRotation -= Input.GetAxis("Mouse Y") * yLookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -upperLookLimit, lowerLookLimit);

        cameraHolder.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * xLookSensitivity, 0 );
    }

    void Headbob()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        if (Mathf.Abs(movementDirection.x) > 0.1f || Mathf.Abs(movementDirection.z) > 0.1f)
        {
            headbobTimer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isRunning ? runningBobSpeed : walkBobSpeed);

            cameraHolder.transform.localRotation = Quaternion.Euler(yDefaultPosition + Mathf.Sin(headbobTimer) * (isCrouching ? crouchBobAmount : isRunning ? runningBobAmount : walkBobAmount) + xRotation,
                (yDefaultPosition + Mathf.Sin(headbobTimer) * (isCrouching ? crouchBobAmount : isRunning ? runningBobAmount : walkBobAmount)) * yAxisMultiplier, cameraHolder.transform.localRotation.z);
        }
    }

    void Crouch()
    {
        if (characterController.height <= standHeight - 0.1f)
        {
            crouchRaycastModifier = crouchCenter.y * 2f;
        }
        else
        {
            crouchRaycastModifier = 0;
        }

        if (shouldCrouch)
        {
            StartCoroutine(CrouchAndStand());
        }
    }

    IEnumerator CrouchAndStand()
    {
        duringCrouchAnimation = true;

        float timeElapsed = 0;

        float targetHeight = isCrouching ? standingHeight : crouchingHeight;

        float currentHeight = characterController.height;

        Vector3 targetCenter = isCrouching ? standingCenter : crouchCenter;

        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        crouchTimer = timeElapsed;

        characterController.height = targetHeight;

        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    void ApplyMovement()
    {
        if (!characterController.isGrounded)
        {
            movementDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(movementDirection * Time.deltaTime);
    }

    IEnumerator SetupDelay()
    {
        yield return null;
        while (VivoxPlayer.Instance.LoginState != LoginState.LoggedIn)
        {
            yield return null;
        }

        yDefaultPosition = cameraHolder.transform.localPosition.y;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        while (!VivoxPlayer.Instance.LocalChannelExists)
        {
            yield return null;
        }

        VivoxPlayer.Instance.LoginSession.SetTransmissionMode(TransmissionMode.Single, VivoxPlayer.Instance.localChannel);
        InvokeRepeating("Update3DPosition", 0, 0.1f);

        overlord = SCR_MultiplayerOverlord.Instance;
    }

    void Update3DPosition()
    {
        if (AmIDead.Value) return;

        try
        {
            VivoxPlayer.Instance.TransmittingSession.Set3DPosition(transform.position, transform.position,
            transform.forward, transform.up);
        }
        catch
        {

        }
    }

    void EnableRadio()
    {
        VivoxPlayer.Instance.LoginSession.SetTransmissionMode(TransmissionMode.Single, VivoxPlayer.Instance.globalChannel);
        nuisance.ToggleEnable(true);
    }

    private void DisableRadio()
    {
        VivoxPlayer.Instance.LoginSession.SetTransmissionMode(TransmissionMode.Single, VivoxPlayer.Instance.localChannel);
        nuisance.ToggleEnable(false);
    }

    [ClientRpc]
    public void PlayerDeathClientRpc() 
    {
        if (!IsOwner)
            return;

        ToggleDeathServerRpc(true);
        StartCoroutine(DieAndGoToSpawn());
    }

    IEnumerator DieAndGoToSpawn()
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.enabled = true;
        canMove = false;

        yield return new WaitForSeconds(2);

        Color c = fader.color;

        for (int i = 0; i < 51; i++)
        {
            c.a = 0.05f * i;
            fader.color = c;

            yield return new WaitForSeconds(0.001f);
        }

        ClientNetworkTransform cnt = GetComponent<ClientNetworkTransform>();
        cnt.Teleport(respawnLocation, Quaternion.identity, transform.localScale);
    }

    [ClientRpc]
    public void PlayerRespawnClientRpc()
    {
        if (!IsOwner)
            return;

        ToggleDeathServerRpc(false);
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        Color c = fader.color;

        canMove = true;

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 101; i++)
        {
            c.a = 1 - 0.01f * i;
            fader.color = c;

            yield return new WaitForSeconds(0.01f);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ToggleDeathServerRpc(bool enableDeath)
    {
        AmIDead.Value = enableDeath;
    }
}
