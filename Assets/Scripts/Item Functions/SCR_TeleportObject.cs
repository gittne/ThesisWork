using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TeleportObject : MonoBehaviour
{

    public Transform teleportTarget;
    public GameObject theRadio;

     void OnTriggerEnter(Collider other)
    {
        theRadio.transform.position = teleportTarget.transform.position;
    }

}
