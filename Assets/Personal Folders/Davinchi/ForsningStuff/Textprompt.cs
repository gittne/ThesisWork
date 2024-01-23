using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Textprompt : MonoBehaviour
{

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
        RaycastHit hit;

        if (Physics.Raycast(playerLookOrigin, playerCamera.transform.forward, out hit, interactionMaxLength, mask))
        {
            GameObject obj = hit.collider.gameObject;
        }
    

}







}
