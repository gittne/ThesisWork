using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SCR_First_Person_Controller : MonoBehaviour
{
    public bool canMove { get; private set; } = true;

    [Header("Movement Variables")]
    [SerializeField] float walkingSpeed = 3f;
    [SerializeField] float gravity = 30f;

    [Header("Mouse Look Variables")]
    [SerializeField, Range(1, 10)] float xLookSensitivity = 2f;
    [SerializeField, Range(1, 10)] float yLookSensitivity = 2f;
    [SerializeField, Range(1, 100)] float upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] float lowerLookLimit = 80f;
    [SerializeField, Range(1, 500)] float middleScreenRadius = 50f;
    Vector2 mousePosition;
    Vector2 screenCenter;
    float distanceFromCenter;

    Camera playerCamera;
    CharacterController characterController;

    Vector3 movementDirection;
    Vector2 currentInput;

    float xRotation = 0;

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateCursorDistance();

        if (canMove)
        {
            MovementInput();

            //MouseLook();

            ApplyMovement();
        }
    }

    void CalculateCursorDistance()
    {
        mousePosition = Input.mousePosition;

        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        distanceFromCenter = Vector2.Distance(mousePosition, screenCenter);

        if (distanceFromCenter > middleScreenRadius)
        {
            Debug.Log("Utanför radien");
        }
        Debug.Log(distanceFromCenter);
    }

    void MovementInput()
    {
        currentInput = new Vector2(walkingSpeed * Input.GetAxis("Vertical"), walkingSpeed * Input.GetAxis("Horizontal"));

        float movementDirectionY = movementDirection.y;

        movementDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);

        movementDirection.y = movementDirectionY;
    }

    void MouseLook()
    {
        xRotation -= Input.GetAxis("Mouse Y") * yLookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -upperLookLimit, lowerLookLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * xLookSensitivity, 0 );
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
