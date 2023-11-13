using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class HapticFeedback
{
    [Range(0, 1)]
    [SerializeField] float vibrationIntensity;
    [SerializeField] float vibrationDuration;

    public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            Vibration(controllerInteractor.xrController);
        }
    }

    void Vibration(XRBaseController controller)
    {
        if (vibrationIntensity > 0)
        {
            controller.SendHapticImpulse(vibrationIntensity, vibrationDuration);
        }
    }
}

public class SCR_HapticFeedback : MonoBehaviour
{
    [SerializeField] HapticFeedback hapticFeedbackOnActivation;
    [SerializeField] HapticFeedback hapticFeedbackOnHoverEnter;
    [SerializeField] HapticFeedback hapticFeedbackOnHoverExit;
    [SerializeField] HapticFeedback hapticFeedbackOnSelectEnter;
    [SerializeField] HapticFeedback hapticFeedbackOnSelectExit;

    private void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();

        interactable.activated.AddListener(hapticFeedbackOnActivation.TriggerHaptic);
        interactable.hoverEntered.AddListener(hapticFeedbackOnHoverEnter.TriggerHaptic);
        interactable.hoverExited.AddListener(hapticFeedbackOnHoverExit.TriggerHaptic);
        interactable.selectEntered.AddListener(hapticFeedbackOnSelectEnter.TriggerHaptic);
        interactable.selectExited.AddListener(hapticFeedbackOnSelectExit.TriggerHaptic);
    }
}
