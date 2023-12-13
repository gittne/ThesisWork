using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_First_Person_Controller_Singleplayer : MonoBehaviour
{
    //SUMMARY: This script is responsible for character movement and looking
    //Base code provided by "Comp-3 Interactive": https://www.youtube.com/watch?v=Ew4l5RPltG8&list=PLfhbBaEcybmgidDH3RX_qzFM0mIxWJa21

    public bool canMove { get; private set; } = true;
    public bool isRunning => canSprintDebug && Input.GetKey(sprintKey);
    public bool shouldCrouch => !duringCrouchAnimation && characterController.isGrounded && Input.GetKey(crouchKey);

    [SerializeField] Camera playerCamera;
    [SerializeField] CharacterController characterController;
    [Header("Functions")]
    [SerializeField] bool canSprintDebug = true;
    [SerializeField] bool canCrouchDebug = true;
    [SerializeField] bool canHeadbobDebug = true;

    [Header("Controls")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode inventoryKey = KeyCode.E;

    [Header("Movement Variables")]
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;
    [SerializeField] float crouchSpeed = 1f;
    [SerializeField] float gravity = 30f;

    [Header("Inventory Variables")]
    [SerializeField] GameObject inventoryPrefab;
    public bool isInventoryActive { get; private set; } = false;
    bool isInventoryInstantiated = false;

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

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inventoryPrefab.SetActive(false);
    }

    void Update()
    {
        InventoryManagement();

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
    }

    void MovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical"),
            (isCrouching ? crouchSpeed : isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal"));

        float movementDirectionY = movementDirection.y;

        movementDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);

        movementDirection.y = movementDirectionY;
    }

    void InventoryManagement()
{
    if (Input.GetKeyDown(inventoryKey))
    {
        isInventoryActive = !isInventoryActive;

        if (isInventoryActive)
        {
            canMove = false;

            if (!isInventoryInstantiated)
            {
                StartCoroutine(SpawnBackpack());
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            canMove = true;

            if (isInventoryInstantiated)
            {
                StartCoroutine(DespawnBackpack());
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}

    IEnumerator SpawnBackpack()
    {
        isInventoryInstantiated = true;

        inventoryPrefab.SetActive(true);

        yield return null;
    }

    IEnumerator DespawnBackpack()
    {
        isInventoryInstantiated = false;

        inventoryPrefab.SetActive(false);

        yield return null;
    }

    void MouseLook()
    {
        xRotation -= Input.GetAxis("Mouse Y") * yLookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -upperLookLimit, lowerLookLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * xLookSensitivity, 0);
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

            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
                yDefaultPosition + Mathf.Sin(headbobTimer) * (isCrouching ? crouchBobAmount : isRunning ? runningBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
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
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
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
}
