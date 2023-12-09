using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Item_Bobbing : NetworkBehaviour
{
    [SerializeField] CharacterController controller;
    [Header("Bobbing Values")]
    [SerializeField] float curveSpeed;
    [SerializeField] Vector3 travelLimit = Vector3.one * 0.025f;
    [SerializeField] Vector3 bobLimit = Vector3.one * 0.01f;

    [SerializeField] float smoothing = 10f;
    [SerializeField] float smoothingRotation = 12f;

    float sinCurve { get => Mathf.Sin(curveSpeed); }
    float cosCurve { get => Mathf.Cos(curveSpeed); }

    Vector3 bobPosition;

    float horizontalInput;
    float verticalInput;
    Vector2 horizontalVerticalInput;

    [Header("Bobbing Rotation Values")]
    [SerializeField] Vector3 multiplier;
    Vector3 eulerRotation;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");
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
        curveSpeed += Time.deltaTime * (controller.isGrounded ? controller.velocity.magnitude : 1f) + 0.01f;

        bobPosition.x = (cosCurve * bobLimit.x * (controller.isGrounded ? 1 : 0)) 
            - (horizontalVerticalInput.x * travelLimit.x);

        bobPosition.y = (sinCurve * bobLimit.y)
            - (controller.velocity.y * travelLimit.y);

        bobPosition.z = -(horizontalVerticalInput.y * travelLimit.z);
    }

    void CompositePositionRotation()
    {

        transform.localPosition = Vector3.Lerp(transform.localPosition, bobPosition, Time.deltaTime * smoothing);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(eulerRotation), Time.deltaTime * smoothingRotation);
    }
}
