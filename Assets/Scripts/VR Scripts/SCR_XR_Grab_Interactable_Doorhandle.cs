using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_XR_Grab_Interactable_Doorhandle : XRGrabInteractable
{
    [System.Serializable]
    public struct ColliderTransformPair
    {
        public Collider collider;
        public Transform attachTransform;
    }

    public ColliderTransformPair[] colliderTransformPairs;

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        // Check the collider on the interactor
        Collider interactorCollider = interactor.GetComponent<Collider>();

        // Iterate through the array to find a match
        foreach (var pair in colliderTransformPairs)
        {
            if (pair.collider == interactorCollider)
            {
                attachTransform = pair.attachTransform;
                return; // exit the loop if a match is found
            }
        }
    }
}
