using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Squeky_Toy_Velocity : MonoBehaviour
{
    [SerializeField] float initialVelocity;

    private void Awake()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.AddForce(Vector3.forward * initialVelocity, ForceMode.Impulse);
    }
}
