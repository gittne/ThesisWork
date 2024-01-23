using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_XR_Socket_Interactor_Backpack : XRSocketInteractor
{
    private Vector3 originalScale;
    [SerializeField] float newScale;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        originalScale = args.interactableObject.transform.localScale;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ScaleObject(args.interactable); 
    }

    private void ScaleObject(XRBaseInteractable interactable)
    {
        interactable.transform.localScale = originalScale;
    }
}