using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using VivoxUnity;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(CharacterController))]
public class SCR_First_Person_Controller : NetworkBehaviour
{
    //SUMMARY: This script is responsible for character movement and looking
    //Base code provided by "Comp-3 Interactive": https://www.youtube.com/watch?v=Ew4l5RPltG8&list=PLfhbBaEcybmgidDH3RX_qzFM0mIxWJa21

    public bool canMove { get; private set; } = true;
    public bool isRunning => canSprintDebug && Input.GetKey(sprintKey);
    public bool shouldCrouch => !duringCrouchAnimation && characterController.isGrounded && Input.GetKey(crouchKey);

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
    float timer;

    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject playerCamera;
    [SerializeField] CharacterController characterController;

    Vector3 movementDirection;
    Vector2 currentInput;

    float xRotation = 0;

    List<GameObject> players = new List<GameObject>();

    void Start()
    {
        if (IsOwner)
        {
            StartCoroutine(SetupDelay());
        }

        StartCoroutine(PlayerListSetup());
    }

    IEnumerator PlayerListSetup()
    {
        while(players.Count < 2)
        {
            foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                players.Add(player);
            }

            if (players.Count < 2)
                players.Clear();

            yield return new WaitForSeconds(0.1f);
        }

        InvokeRepeating("PlayerStatusUpdate", 0, 0.1f);
    }

    // Update is called once per frame
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
        }

        if (Input.GetKeyDown(radioKey)) EnableRadio();
        if (Input.GetKeyUp(radioKey)) DisableRadio();
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
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isRunning ? runningBobSpeed : walkBobSpeed);

            cameraTransform.transform.localPosition = new Vector3(cameraTransform.transform.localPosition.x, 
                yDefaultPosition + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isRunning ? runningBobAmount : walkBobAmount), 
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
    }

    void GoUpdatePosition()
    {
        if (!VivoxPlayer.Instance.LoginSession.GetChannelSession(VivoxPlayer.Instance.localChannel).IsTransmitting)
            return;

        Update3DPosition(transform, transform);
    }

    void PlayerStatusUpdate()
    {
        foreach(GameObject player in players)
        {
            if (player.GetComponent<SCR_Flashlight_Non_VR>().FlashLightIsOn)
                player.GetComponent<Light>().intensity = 1;
            else
                player.GetComponent<Light>().intensity = 0;
        }
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
}
