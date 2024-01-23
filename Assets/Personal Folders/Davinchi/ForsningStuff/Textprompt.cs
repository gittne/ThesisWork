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

    [SerializeField] TextMeshProUGUI TextPrompt;

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
            
            if (obj.TryGetComponent(out SCR_Key_Card_Reader _Keyread))
            {
                

                if (_Keyread.canActivate == false && _Keyread.isActivated == false)
                    TextPrompt.text = "Power needed";
                else 
                    TextPrompt.text = "Card needed";
                if (_Keyread.canActivate == true && _Keyread.isActivated == true)
                    TextPrompt.text = "";


            }

            if (obj.TryGetComponent(out SCR_FuseBox _FuseBox))
            {


                if (_FuseBox.isActivated == false)
                    TextPrompt.text = "Fuses needed";
                else
                    TextPrompt.text = "";
                


            }
        }
        else
            TextPrompt.text = "";

    }







}
