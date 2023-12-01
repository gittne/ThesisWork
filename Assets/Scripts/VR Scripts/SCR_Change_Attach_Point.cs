using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_Change_Attach_Point : MonoBehaviour
{
    enum HandType { Left, Right }

    [SerializeField] HandType handType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out SCR_XR_Grab_Interactable_Alternate_Attach leftGrabInteractable) && handType == HandType.Left)
        {
            leftGrabInteractable.ChangeAttachPointLeft();
        }

        else if (other.gameObject.TryGetComponent(out SCR_XR_Grab_Interactable_Alternate_Attach rightGrabInteractable) && handType == HandType.Right)
        {
            rightGrabInteractable.ChangeAttachPointRight();
        }
    }
}
