using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_XR_Socket_Interactor_Backpack : XRSocketInteractor
{
    private Vector3 originalScale;
    [SerializeField] float newScale;
    Renderer objectRenderer;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        originalScale = args.interactableObject.transform.localScale;

        objectRenderer = args.interactable.GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            SetShadows(false);
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ScaleObject(args.interactable);

        if (objectRenderer != null)
        {
            SetShadows(true);

            objectRenderer = null;
        }
    }

    private void ScaleObject(XRBaseInteractable interactable)
    {
        interactable.transform.localScale = originalScale;
    }

    private void SetShadows(bool enableShadows)
    {
        if (objectRenderer != null)
        {
            objectRenderer.shadowCastingMode = enableShadows ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
}