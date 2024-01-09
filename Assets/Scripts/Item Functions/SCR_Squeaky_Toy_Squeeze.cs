using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Squeaky_Toy_Squeeze : MonoBehaviour
{
    Rigidbody objectRigidbody;
    [SerializeField] LayerMask hittableMask;
    [Header("Sound Varaibles")]
    AudioSource soundSource;
    [SerializeField] AudioClip squeezeSound;
    [SerializeField] float resetTimer;
    bool canMakeSound;
    [Header("Velocity Varaibles")]
    [SerializeField] float minimumActivationVelocity;
    [Header("Animation Varaibles")]
    [SerializeField] Animator animator;
    [SerializeField] float animationSpeed;
    [SerializeField] string animationName;
    [Header("Ray Varaibles")]
    [SerializeField] float xRayDistance;
    [SerializeField] float yRayDistance;
    [SerializeField] float zRayDistance;
    [SerializeField] Transform rayTransform;

    // Start is called before the first frame update
    void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();

        soundSource = GetComponent<AudioSource>();

        animator.enabled = true;

        animator.speed = animationSpeed;

        canMakeSound = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded() && objectRigidbody.velocity.magnitude > minimumActivationVelocity)
        {
            animator.SetTrigger(animationName);

            if (canMakeSound)
            {
                StartCoroutine(PlaySound());
            }
        }

        Debug.DrawRay(rayTransform.position, transform.forward * zRayDistance, Color.red);
        Debug.DrawRay(rayTransform.position, -transform.forward * zRayDistance, Color.red);
        Debug.DrawRay(rayTransform.position, transform.right * xRayDistance, Color.red);
        Debug.DrawRay(rayTransform.position, -transform.right * xRayDistance, Color.red);
        Debug.DrawRay(rayTransform.position, transform.up * yRayDistance, Color.red);
        Debug.DrawRay(rayTransform.position, -transform.up * yRayDistance, Color.red);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;

        return Physics.Raycast(rayTransform.position, -transform.forward, out hit, zRayDistance, hittableMask)
            || Physics.Raycast(rayTransform.position, transform.forward, out hit, zRayDistance, hittableMask) 
            || Physics.Raycast(rayTransform.position, transform.right, out hit, xRayDistance, hittableMask)
            || Physics.Raycast(rayTransform.position, -transform.right, out hit, xRayDistance, hittableMask)
            || Physics.Raycast(rayTransform.position, transform.up, out hit, yRayDistance, hittableMask)
            || Physics.Raycast(rayTransform.position, -transform.up, out hit, yRayDistance, hittableMask);
    }

    IEnumerator PlaySound()
    {
        float randomPitch = Random.Range(0.8f, 1.2f);
        soundSource.pitch = randomPitch;
        soundSource.PlayOneShot(squeezeSound);
        canMakeSound = false;
        yield return new WaitForSeconds(resetTimer);
        canMakeSound = true;
    }
}
