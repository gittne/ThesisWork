using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Player_Interactions : MonoBehaviour
{
    KeyCode interactionKey = KeyCode.E;
    LayerMask mask;
    Vector3 playerLookOrigin;

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
            Interact();

        playerLookOrigin = transform.position + new Vector3(0, 1, 0);
    }

    void Interact()
    {
        /* Interaction types:
         * Doors, cupboards (play animation)
         * Pickup items
         * Buttons, generators, keycards (locked doors)
         */

        RaycastHit hit;

        if(Physics.Raycast(playerLookOrigin, Vector3.forward, out hit, 1, mask)) 
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
                if(inter.IsEnabled)
                {
                    inter.Interact();
                }
            }
        }

    }
}
