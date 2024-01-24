using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_XR_Socket_Fuse_Check : XRSocketInteractor
{
    [SerializeField] string targetTag;
    bool isInserted;
    public bool isFuseInserted
    {
        get { return isInserted; }
        private set { isInserted = value; }
    }

    [System.Obsolete]
    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        base.OnSelectEntered(interactable);

        if (interactable.CompareTag(targetTag))
        {
            isInserted = true;
        }
    }
}
