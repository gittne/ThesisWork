using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Player_Interactions : MonoBehaviour
{
    [SerializeField] KeyCode interactionKey = KeyCode.E;
    [SerializeField] LayerMask mask;
    Vector3 playerLookOrigin;
    [SerializeField] Camera playerCamera;
    [SerializeField] float interactionMaxLength;

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
            Interact();

        playerLookOrigin = playerCamera.transform.position;
    }

    void Interact()
    {
        /* Interaction types:
         * Doors, cupboards (play animation)
         * Pickup items
         * Buttons, generators, keycards (locked doors)
         */

        RaycastHit hit;

        if(Physics.Raycast(playerLookOrigin, playerCamera.transform.forward, out hit, interactionMaxLength, mask)) 
        {
            GameObject obj = hit.collider.gameObject;

            if(obj.TryGetComponent(out SCR_Inventory_Pickup_Singeplayer pickup))
            {
                pickup.OnHandlePickupItem();
            }

            if(obj.TryGetComponent(out SCR_Animated_Interactable anim))
            {
                anim.SwitchAnimationState();
            }

            if(obj.TryGetComponent(out SCR_Switch_Interactable inter))
            {
                if(!inter.IsEnabled)
                {
                    inter.Interact();
                }
            }
        }
    }
}
