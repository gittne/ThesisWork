using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_Player_Interactions : MonoBehaviour
{
    [SerializeField] KeyCode interactionKey = KeyCode.E;
    [SerializeField] LayerMask mask;
    Vector3 playerLookOrigin;
    [SerializeField] Camera playerCamera;
    [SerializeField] float interactionMaxLength;


    void Update()
    {
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

            if (obj.TryGetComponent(out SCR_Inventory_Pickup_Singeplayer pickup))
            {
               

                if (Input.GetKeyDown(interactionKey))
                {
                    pickup.OnHandlePickupItem();
                }
            }

            if (obj.TryGetComponent(out SCR_Inventory_Pickup_Multiplayer pickupMultiplayer))
            {
                

                if (Input.GetKeyDown(interactionKey))
                {
                    pickupMultiplayer.OnHandlePickupItemServerRPC();
                }
            }

            if (obj.TryGetComponent(out SCR_Animated_Interactable_Multiplayer animM))
            {

                if (Input.GetKeyDown(interactionKey))
                {
                    animM.SwitchAnimationState();
                }
            }


            if (obj.TryGetComponent(out SCR_Animated_Interactable anim))
            {


                if (Input.GetKeyDown(interactionKey))
                {
                    anim.SwitchAnimationState();
                }
            }


        }
    }
}
