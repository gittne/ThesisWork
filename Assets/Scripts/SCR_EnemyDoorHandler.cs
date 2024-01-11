using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EnemyDoorHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Door"))
        {
            other.gameObject.GetComponent<SCR_Animated_Interactable>().MonsterOpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            other.gameObject.GetComponent<SCR_Animated_Interactable>().MonsterCloseDoor();
        }
    }
}
