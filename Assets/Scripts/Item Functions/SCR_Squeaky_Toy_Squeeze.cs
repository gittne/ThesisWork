using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Squeaky_Toy_Squeeze : MonoBehaviour
{
    Animator animator;
    Rigidbody objectRigidbody;
    [SerializeField] float rayDistance;
    [SerializeField] float minimumActivationVelocity;
    [SerializeField] string animationName;
    [SerializeField] Transform rayTransform;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        objectRigidbody = GetComponent<Rigidbody>();

        animator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded() && objectRigidbody.velocity.magnitude > minimumActivationVelocity)
        {
            animator.SetTrigger(animationName);

            Debug.Log("Has touched the ground");
        }

        Debug.DrawRay(rayTransform.position, transform.forward * rayDistance, Color.red);
        Debug.DrawRay(rayTransform.position, -transform.forward * rayDistance, Color.red);
        Debug.DrawRay(rayTransform.position, transform.right * rayDistance, Color.red);
        Debug.DrawRay(rayTransform.position, -transform.right * rayDistance, Color.red);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;

        return Physics.Raycast(rayTransform.position, -transform.forward, out hit, rayDistance)
            || Physics.Raycast(rayTransform.position, transform.forward, out hit, rayDistance) 
            || Physics.Raycast(rayTransform.position, transform.right, out hit, rayDistance)
            || Physics.Raycast(rayTransform.position, -transform.right, out hit, rayDistance);
    }
}
