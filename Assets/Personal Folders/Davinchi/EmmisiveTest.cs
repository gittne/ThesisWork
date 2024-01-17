using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmisiveTest : MonoBehaviour
{
    Material MAT;

    private void Start()
    {
        MAT = gameObject.GetComponent<Renderer>().material; 
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("emissiveOn");

            MAT.SetColor("_EmissionColor", Color.black);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("changecolor");

            MAT.SetColor("_EmissionColor", Color.red);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("changecolor1");

            MAT.SetColor("_EmissionColor", Color.green);
        }




    }
}
