using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_AnimateHandOnInput : MonoBehaviour
{
    [SerializeField] InputActionProperty pinchAction;
    [SerializeField] InputActionProperty gripAction;
    [SerializeField] Animator handAnimator;

    void Update()
    {
        float triggerValue = pinchAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }
}
