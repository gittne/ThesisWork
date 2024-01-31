using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Item_Bobbing : NetworkBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] SCR_First_Person_Controller controllerScript;
    [Header("Which hand is holding")]
    [SerializeField] bool isInLeftHand;
    [Header("Bobbing Values")]
    float curveSpeed;
    [SerializeField] Vector3 travelLimit = Vector3.one * 0.025f;
    [SerializeField] Vector3 bobLimit = Vector3.one * 0.01f;
    [Header("Smoothing Values")]
    [SerializeField] float smoothing = 10f;
    [SerializeField] float smoothingRotation = 12f;
    [Header("Movement Multipliers")]
    [SerializeField] float movementMultiplier;

    float sinCurve { get => Mathf.Sin(curveSpeed); }
    float cosCurve { get => Mathf.Cos(curveSpeed); }

    Vector3 bobPosition;

    float horizontalInput;
    float verticalInput;
    Vector2 horizontalVerticalInput;

    [Header("Bobbing Rotation Values")]
    [SerializeField] Vector3 multiplier;
    Vector3 eulerRotation;

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (!isInLeftHand)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }
        else
        {
            horizontalInput = -Input.GetAxis("Horizontal");
        }

        verticalInput = Input.GetAxis("Vertical");

        horizontalVerticalInput.x = horizontalInput;
        horizontalVerticalInput.y = verticalInput;

        BobOffset();
        BobRotation();
        CompositePositionRotation();
    }

    void BobRotation()
    {
        eulerRotation.x = (horizontalVerticalInput != Vector2.zero ?
            multiplier.x * (Mathf.Sin(2 * curveSpeed)) : multiplier.x * (Mathf.Sin(2 * curveSpeed)) / 2);

        eulerRotation.y = (horizontalVerticalInput != Vector2.zero ? multiplier.y * cosCurve : 0);

        eulerRotation.z = (horizontalVerticalInput != Vector2.zero ? multiplier.z * cosCurve * horizontalVerticalInput.x : 0);
    }

    void BobOffset()
    {
        curveSpeed += Time.deltaTime * (controller.isGrounded ? controller.velocity.magnitude * movementMultiplier : 1f) + 0.01f;

        bobPosition.x = (cosCurve * bobLimit.x * (controller.isGrounded ? 1 : 0))
            - (horizontalVerticalInput.x * travelLimit.x);

        bobPosition.y = (sinCurve * bobLimit.y)
            - (controller.velocity.y * travelLimit.y);

        bobPosition.z = -(horizontalVerticalInput.y * travelLimit.z);
    }

    void CompositePositionRotation()
    {
        if (controller.height < controllerScript.standHeight && controller.height > controllerScript.crouchHeight || controller.velocity.y != 0)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition, 1500f * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, bobPosition, (controller.velocity.magnitude > 0.1f ? controller.velocity.magnitude * Time.deltaTime : smoothing * Time.deltaTime));
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(eulerRotation), (controller.velocity.magnitude > 0.1 ? controller.velocity.magnitude * Time.deltaTime : smoothingRotation * Time.deltaTime));
    }
}
