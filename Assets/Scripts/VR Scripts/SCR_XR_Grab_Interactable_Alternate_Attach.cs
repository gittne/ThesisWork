using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_XR_Grab_Interactable_Alternate_Attach : XRGrabInteractable
{
    [SerializeField] Transform leftAttachTransform;
    [SerializeField] Transform rightAttachTransform;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachTransform;
        }
        else if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightAttachTransform;
        }

        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachTransform;
        }
        else if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightAttachTransform;
        }

        base.OnSelectExited(args);
    }

    public void ChangeAttachPointLeft()
    {
        attachTransform = leftAttachTransform;
    }

    public void ChangeAttachPointRight()
    {
        attachTransform = rightAttachTransform;
    }
}
