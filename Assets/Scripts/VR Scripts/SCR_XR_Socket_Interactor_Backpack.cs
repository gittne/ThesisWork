using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScaleOnGrab : XRSocketInteractor
{
    private Vector3 originalScale;
    [SerializeField] float newScale;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        originalScale = args.interactable.transform.localScale;
        ScaleObject(args.interactable, newScale); 
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ScaleObject(args.interactable, originalScale.x); 
    }

    private void ScaleObject(XRBaseInteractable interactable, float scaleFactor)
    {
        interactable.transform.localScale = originalScale * scaleFactor;
    }
}