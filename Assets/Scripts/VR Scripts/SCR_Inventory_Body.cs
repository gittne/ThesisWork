using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class InventorySocket
{
    public enum BodyPart { Hips, Chest }
    [Tooltip("Where on the body the socket is attached.")]
    public BodyPart bodyPart;

    [Tooltip("The socket objects attached to this object.")]
    [SerializeField] GameObject socketObjects;
    [HideInInspector] public GameObject gameObject { get { return socketObjects; } }

    [Tooltip("The height ratio of the sockets compared to the height of the player when socket is attached to hip.")]
    [Range(0f, 1f)]
    [SerializeField] float heightRatio;
    [HideInInspector] public float socketHeightRatio { get { return heightRatio; } }

    [Tooltip("The amount to deduct from player height when socket is attached to chest.")]
    [SerializeField] float heightDeduction;
    [HideInInspector] public float socketHeightDeduction { get { return heightDeduction; } }
}

public class SCR_Inventory_Body : MonoBehaviour
{
    [Header("HMD (Head Mounted Display)")]
    [SerializeField] GameObject headset;
    [SerializeField] InputActionReference stickInput;
    private bool isRotating = false;
    private float startTime;

    [Header("Inventory Sockets")]
    [SerializeField] InventorySocket[] sockets;

    Vector3 headsetPosition;
    Quaternion headsetRotation;

    void OnEnable()
    {
        stickInput.action.Enable();
        stickInput.action.started += OnRotateStarted;
        stickInput.action.canceled += OnRotateCanceled;
    }

    void OnDisable()
    {
        stickInput.action.Disable();
        stickInput.action.started -= OnRotateStarted;
        stickInput.action.canceled -= OnRotateCanceled;
    }

    void Update()
    {
        headsetPosition = headset.transform.localPosition;
        headsetRotation = headset.transform.localRotation;

        foreach (var bodySocket in sockets)
        {
            UpdateSocketHeight(bodySocket);
        }

        UpdateSocketPosition();

        if (isRotating)
        {
            float timeElapsed = Time.time - startTime;
            float t = Mathf.Sin(timeElapsed * Mathf.PI * 0.5f);

            headset.transform.rotation = Quaternion.Euler(0f, t * 90f, 0f);

            if (timeElapsed >= 0.5f)
            {
                isRotating = false;
            }
        }
    }

    void UpdateSocketHeight(InventorySocket socket)
    {
        switch (socket.bodyPart)
        {
            case InventorySocket.BodyPart.Hips:
                socket.gameObject.transform.localPosition = new Vector3(socket.gameObject.transform.localPosition.x,
                (headsetPosition.y * socket.socketHeightRatio), socket.gameObject.transform.localPosition.z);
                break;
            case InventorySocket.BodyPart.Chest:
                socket.gameObject.transform.localPosition = new Vector3(socket.gameObject.transform.localPosition.x,
                (headsetPosition.y - socket.socketHeightDeduction), socket.gameObject.transform.localPosition.z);
                break;
            default:
                Debug.LogError("Error with bodypart enum");
                break;
        }
    }

    void UpdateSocketPosition()
    {
        transform.localPosition = new Vector3(headsetPosition.x, 0, headsetPosition.z);
        transform.localRotation = new Quaternion(transform.rotation.x, headsetRotation.y, transform.rotation.z, headsetRotation.w);
    }

    void OnRotateStarted(InputAction.CallbackContext context)
    {
        isRotating = true;
        startTime = Time.time;
    }

    void OnRotateCanceled(InputAction.CallbackContext context)
    {
        isRotating = false;
    }
}
