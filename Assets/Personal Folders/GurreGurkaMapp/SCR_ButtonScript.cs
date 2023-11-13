using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ButtonScript : MonoBehaviour
{
    [SerializeField] Ray activationRay;
    [SerializeField] float activationRayLength;
    bool isActivated;
    public bool hasBeenActivated { get { return isActivated; } }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(activationRay, out hit, activationRayLength))
        {

        }
    }
}
