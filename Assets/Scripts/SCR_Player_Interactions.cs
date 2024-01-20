using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class SCR_Player_Interactions : NetworkBehaviour
{
    [SerializeField] KeyCode interactionKey = KeyCode.E;
    [SerializeField] LayerMask mask;
    Vector3 playerLookOrigin;
    [SerializeField] Camera playerCamera;
    [SerializeField] float interactionMaxLength;
    GameObject interactTextCanvas;
    GameObject keyLockTextCanvas;

    private void Awake()
    {
        interactTextCanvas = GameObject.FindGameObjectWithTag("InteractionText");
        keyLockTextCanvas = GameObject.FindGameObjectWithTag("KeyLockText");
        interactTextCanvas.SetActive(false);
        keyLockTextCanvas.SetActive(false);
    }

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
                interactTextCanvas.SetActive(true);

                if (Input.GetKeyDown(interactionKey))
                {
                    pickup.OnHandlePickupItem();
                }
            }

            if (obj.TryGetComponent(out SCR_Inventory_Pickup_Multiplayer pickupMultiplayer))
            {
                interactTextCanvas.SetActive(true);

                if (Input.GetKeyDown(interactionKey))
                {
                    pickupMultiplayer.OnHandlePickupItem();
                }
            }

            if (obj.TryGetComponent(out SCR_Animated_Interactable anim))
            {
                if (anim.lockStatus == SCR_Animated_Interactable.LockState.Locked)
                {
                    keyLockTextCanvas.SetActive(true);
                }
                else
                {
                    interactTextCanvas.SetActive(true);
                }

                if (Input.GetKeyDown(interactionKey))
                {
                    anim.SwitchAnimationState();
                }
            }

            if (obj.TryGetComponent(out SCR_Switch_Interactable inter))
            {
                if (!inter.IsEnabled)
                {
                    interactTextCanvas.SetActive(true);
                    
                }
            }

            if (obj.TryGetComponent(out SCR_TempWinScript Tempwin)) // Temporary script for ending the game
            {
                if (Input.GetKeyDown(interactionKey))
                {
                    Tempwin.VictoryScreen();
                }
            }
        }
        else
        {
            keyLockTextCanvas.SetActive(false);
            interactTextCanvas.SetActive(false);
        }
    }
}
