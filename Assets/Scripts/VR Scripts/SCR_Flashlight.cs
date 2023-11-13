using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_Flashlight : MonoBehaviour
{
    [Header("Light Sources")]
    [SerializeField] Light spotLight;
    [SerializeField] Light lightBulb;

    [Header("Controller Input Binding")]
    [SerializeField] InputActionProperty flipLightButton;

    [Header("Rotation Angle")]
    [SerializeField] Vector3 rotationAngles;
    XRGrabInteractable grabbable;

    // Start is called before the first frame update
    void Start()
    {
        spotLight.enabled = false;
        lightBulb.enabled = false;

        grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(TurnOnOrOff);
    }

    void TurnOnOrOff(ActivateEventArgs arg)
    {
        spotLight.enabled = !spotLight.enabled;
        lightBulb.enabled = !lightBulb.enabled;
    }

    void Flip()
    {
        
    }
}
