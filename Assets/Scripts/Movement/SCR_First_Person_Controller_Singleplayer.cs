using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_First_Person_Controller_Singleplayer : MonoBehaviour
{
    //SUMMARY: This script is responsible for character movement and looking
    //Base code provided by "Comp-3 Interactive": https://www.youtube.com/watch?v=Ew4l5RPltG8&list=PLfhbBaEcybmgidDH3RX_qzFM0mIxWJa21

    public bool canMove { get; private set; } = true;
    public float crouchTimer { get; private set; }
    public bool isRunning => canSprintDebug && Input.GetKey(sprintKey);
    public bool shouldCrouch => !duringCrouchAnimation && characterController.isGrounded && Input.GetKeyDown(crouchKey);

    [SerializeField] Camera playerCamera;
    [SerializeField] Transform cameraHolder;
    [SerializeField] CharacterController characterController;
    [Header("Functions")]
    [SerializeField] bool canSprintDebug = true;
    [SerializeField] bool canCrouchDebug = true;
    [SerializeField] bool canHeadbobDebug = true;

    [Header("Controls")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement Variables")]
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;
    [SerializeField] float crouchSpeed = 1f;
    [SerializeField] float gravity = 30f;

    [Header("Inventory Variables")]
    [SerializeField] SCR_Inventory_Visual visualInventory;

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

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
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
    }

    void MovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical"),
            (isCrouching ? crouchSpeed : isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal"));

        float movementDirectionY = movementDirection.y;

        movementDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);

        movementDirection.y = movementDirectionY;
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

            cameraHolder.transform.localRotation = Quaternion.Euler(yDefaultPosition + Mathf.Sin(headbobTimer) * (isCrouching ? crouchBobAmount : isRunning ? runningBobAmount : walkBobAmount),
                (yDefaultPosition + Mathf.Sin(headbobTimer) * (isCrouching ? crouchBobAmount : isRunning ? runningBobAmount : walkBobAmount)) * yAxisMultiplier, cameraHolder.transform.localRotation.z);
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

    public void PlayerDie(GameObject monster)
    {
        canMove = false;
        transform.position += new Vector3(0, 0.35f, 0);
        StartCoroutine(Fall(monster));
    }

    IEnumerator Fall(GameObject monster)
    {
        yield return new WaitForSeconds(2);
        while(cameraHolder.transform.position.y > 0.15f)
        {

            cameraHolder.LookAt(monster.transform.position + new Vector3(0, 1.5f, 0));
            cameraHolder.transform.position -= new Vector3(0, 0.01f, 0);
            yield return new WaitForSeconds(0.000000001f);
            Debug.Log("FALL");
        }
    }
}
