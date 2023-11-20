using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Mirror : MonoBehaviour
{
    [SerializeField] GameObject Mirror;
    Material MirrorMat;

    private void Start()
    {
        MirrorMat = Mirror.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MirrorMat.SetFloat("_Strenght", 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MirrorMat.SetFloat("_Strenght", 0.001f);
        }
    }


}
