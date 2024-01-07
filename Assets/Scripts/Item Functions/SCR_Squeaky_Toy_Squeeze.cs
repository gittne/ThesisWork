using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Squeaky_Toy_Squeeze : MonoBehaviour
{
    Rigidbody objectRigidbody;
    [Header("Sound Varaibles")]
    AudioSource soundSource;
    [SerializeField] AudioClip squeezeSound;
    [Header("Velocity Varaibles")]
    [SerializeField] float minimumActivationVelocity;
    [Header("Animation Varaibles")]
    [SerializeField] Animator animator;
    [SerializeField] float animationSpeed;
    [SerializeField] string animationName;
    [Header("Ray Varaibles")]
    [SerializeField] float rayDistance;
    [SerializeField] Transform rayTransform;

    // Start is called before the first frame update
    void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();

        soundSource = GetComponent<AudioSource>();

        animator.enabled = true;

        animator.speed = animationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded() && objectRigidbody.velocity.magnitude > minimumActivationVelocity)
        {
            animator.SetTrigger(animationName);
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
