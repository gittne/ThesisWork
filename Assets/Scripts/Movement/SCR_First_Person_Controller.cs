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

[RequireComponent(typeof(CharacterController))]
public class SCR_First_Person_Controller : NetworkBehaviour
{
    //SUMMARY: This script is responsible for character movement and looking
    //Base code provided by "Comp-3 Interactive": https://www.youtube.com/watch?v=Ew4l5RPltG8&list=PLfhbBaEcybmgidDH3RX_qzFM0mIxWJa21

    public bool canMove { get; private set; } = true;
    public bool isRunning => canSprintDebug && Input.GetKey(sprintKey);
    public bool shouldCrouch => !duringCrouchAnimation && characterController.isGrounded && Input.GetKey(crouchKey);

    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject playerCamera;
    [SerializeField] Transform cameraHolder;
    [SerializeField] CharacterController characterController;
    [Header("Functions")]
    [SerializeField] Vector3 spawnpoint = new Vector3(0, 0, 0);
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

    [Header("Mouse Look Variables")]
    [SerializeField, Range(1, 10)] float xLookSensitivity = 2f;
    [SerializeField, Range(1, 10)] float yLookSensitivity = 2f;
    [SerializeField, Range(1, 100)] float upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] float lowerLookLimit = 80f;

    [Header("Crouching Variables")]
    [SerializeField] float crouchHeight;
    [SerializeField] float standingHeight;
    [SerializeField] float timeToCrouch;
    [SerializeField] Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] Vector3 standingCenter = new Vector3(0, 0, 0);
    bool isCrouching;
    bool duringCrouchAnimation;

    [Header("Headbob Variables")]
    [SerializeField] float walkBobSpeed;
    [SerializeField] float walkBobAmount;
    [SerializeField] float runningBobSpeed;
    [SerializeField] float runningBobAmount;
    [SerializeField] float crouchBobSpeed;
    [SerializeField] float crouchBobAmount;
    float yDefaultPosition = 0;
    float headbobTimer;

    [Header("Footstep Variables")]
    [SerializeField] float baseStepSpeed;
    [SerializeField] float crouchStepMultiplier;
    [SerializeField] float runStepMultiplier;
    [SerializeField] AudioSource audioSource = default;
    [SerializeField] AudioClip[] carpetClips = default;
    [SerializeField] AudioClip[] woodClips = default;
    [SerializeField] AudioClip[] stoneClips = default;
    float footstepTimer;
    float getCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isRunning ? baseStepSpeed * runStepMultiplier : baseStepSpeed;

    Vector3 movementDirection;
    Vector2 currentInput;

    float xRotation = 0;

    Image fader;

    SCR_MultiplayerOverlord overlord;

    bool isDead = false;
    public bool IsDead { get { return isDead; }}

    void Start()
    {
        if (!IsOwner) return;

        StartCoroutine(SetupDelay());
        fader = GameObject.FindGameObjectWithTag("BlackFade").GetComponent<Image>();
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (canMove)
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

            HandleFootsteps();
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

        cameraTransform.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

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

            cameraTransform.transform.localPosition = new Vector3(cameraTransform.transform.localPosition.x, 
                yDefaultPosition + Mathf.Sin(headbobTimer) * (isCrouching ? crouchBobAmount : isRunning ? runningBobAmount : walkBobAmount), 
                cameraTransform.transform.localPosition.z);
        }
    }

    void Crouch()
    {
        if (shouldCrouch)
        {
            StartCoroutine(CrouchAndStand());
        }
    }

    IEnumerator CrouchAndStand()
    {
        duringCrouchAnimation = true;

        float timeElapsed = 0;

        float targetHeight = isCrouching ? standingHeight : crouchHeight;

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

        characterController.height = targetHeight;

        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    void HandleFootsteps()
    {
        if (!characterController.isGrounded || currentInput == Vector2.zero)
        {
            return;
        }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(cameraTransform.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Material/Fabric":
                        audioSource.PlayOneShot(carpetClips[Random.Range(0, carpetClips.Length - 1)]);
                        break;
                    case "Material/Wood":
                        audioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Material/Stone":
                        audioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                        break;
                    default:
                        break;
                }
            }

            footstepTimer = getCurrentOffset;
        }
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
        while (VivoxPlayer.Instance.LoginState != LoginState.LoggedIn)
        {
            yield return null;
        }

        transform.position = spawnpoint;

        yDefaultPosition = cameraTransform.transform.localPosition.y;

        if (Instantiate(playerCamera, cameraTransform))
        {
            Debug.LogWarning("Kamera instantierad");
        }
        else
        {
            Debug.LogWarning("Kamera inte instantierad");
        }


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        while(!VivoxPlayer.Instance.LocalChannelExists)
        {
            yield return null;
        }
        
        VivoxPlayer.Instance.LoginSession.SetTransmissionMode(TransmissionMode.Single, VivoxPlayer.Instance.localChannel);
        InvokeRepeating("GoUpdatePosition", 0, 0.1f);
        overlord = SCR_MultiplayerOverlord.Instance;
    }

    void GoUpdatePosition()
    {
        if (!VivoxPlayer.Instance.LoginSession.GetChannelSession(VivoxPlayer.Instance.localChannel).IsTransmitting)
            return;

        Update3DPosition(transform, transform);
    }

    void Update3DPosition(Transform listener, Transform speaker)
    {
        try
        {
            VivoxPlayer.Instance.TransmittingSession.Set3DPosition(speaker.position, listener.position,
            listener.forward, listener.up);
        }
        catch
        {

        }
    }

    void EnableRadio()
    {
        VivoxPlayer.Instance.LoginSession.SetTransmissionMode(TransmissionMode.Single, VivoxPlayer.Instance.globalChannel);
    }

    private void DisableRadio()
    {
        VivoxPlayer.Instance.LoginSession.SetTransmissionMode(TransmissionMode.Single, VivoxPlayer.Instance.localChannel);
    }

    [ClientRpc]
    public void PlayerDeathClientRpc() 
    {
        if (!IsOwner)
            return;

        ClientNetworkTransform cnt = GetComponent<ClientNetworkTransform>();
        cnt.Teleport(new Vector3(0, 1, 0), Quaternion.identity, transform.localScale);
    }

    public void CommencePlayerDeath(GameObject monster)
    {
        if (!IsOwner)
            return;

        StartCoroutine(DieAndGoToSpawn(monster));
    }

    IEnumerator DieAndGoToSpawn(GameObject monster)
    {
        isDead = true;
        canMove = false;
        transform.position += new Vector3(0, 0.35f, 0);

        yield return new WaitForSeconds(2);
        while (cameraHolder.transform.position.y > 0.15f)
        {

            cameraHolder.LookAt(monster.transform.position + new Vector3(0, 1.5f, 0));
            cameraHolder.transform.position -= new Vector3(0, 0.03f, 0);
            yield return new WaitForSeconds(0.001f);
        }

        Color c = fader.color;

        for (int i = 0; i < 51; i++)
        {
            c.a = 0.05f * i;
            fader.color = c;

            yield return new WaitForSeconds(0.001f);
        }

        ClientNetworkTransform cnt = GetComponent<ClientNetworkTransform>();
        cnt.Teleport(new Vector3(0, 1, 0), Quaternion.identity, transform.localScale);

        cameraHolder.transform.position += new Vector3(0, 1.7f, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        cameraHolder.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void GoRespawn()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        Color c = fader.color;

        for (int i = 0; i < 101; i++)
        {
            c.a = 1 - 0.01f * i;
            fader.color = c;

            yield return new WaitForSeconds(0.01f);
        }

        canMove = true;
    }
}
