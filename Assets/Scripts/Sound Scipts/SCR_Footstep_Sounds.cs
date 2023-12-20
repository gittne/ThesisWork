using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Footstep_Sounds : MonoBehaviour
{
    [Header("Footstep Variables")]
    [SerializeField] float stepSpeed;
    [SerializeField] AudioSource audioSource = default;
    [SerializeField] AudioClip[] carpetClips = default;
    [SerializeField] AudioClip[] woodClips = default;
    [SerializeField] AudioClip[] stoneClips = default;
    [SerializeField] AudioClip[] monsterSteps = default;
    float footStepThreshold;
    [SerializeField] float footstepTimer;
    GameObject characterObject;

    // Start is called before the first frame update
    void Start()
    {
        characterObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        PlayFootsteps();
        footstepTimer -= Time.deltaTime;
        //Debug.Log(footStepThreshold);
        Debug.Log(footstepTimer);
    }

    void PlayFootsteps()
    {
        if (characterObject == null)
        {
            Debug.LogError("No valid component found");
            return;
        }

        if (characterObject.TryGetComponent(out Rigidbody rigidbody))
        {
            footStepThreshold = stepSpeed / new Vector2(rigidbody.velocity.x, rigidbody.velocity.z).magnitude;

            //footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                if (rigidbody.velocity != new Vector3(0, 0, 0)
                && Physics.Raycast(rigidbody.transform.position, Vector3.down, out RaycastHit hit, 3f))
                {
                    switch (hit.collider.tag)
                    {
                        default:
                            audioSource.PlayOneShot(carpetClips[Random.Range(0, monsterSteps.Length - 1)]);
                            break;
                    }
                }
            }

            footstepTimer = footStepThreshold;
        }
        else if(characterObject.TryGetComponent(out CharacterController controller))
        {
            footStepThreshold = stepSpeed / Mathf.Abs(new Vector2(controller.velocity.x, controller.velocity.z).magnitude);

            //footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                if (Physics.Raycast(controller.transform.position, Vector3.down, out RaycastHit hit, 3f))
                {
                    switch (hit.collider.tag)
                    {
                        case "Material/Fabric":
                            audioSource.PlayOneShot(carpetClips[Random.Range(0, carpetClips.Length - 1)]);
                            break;
                        case "Material/Wood":
                            audioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                            break;
                        case "Material/Stone":
                            audioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                            break;
                        default:
                            break;
                    }
                }
                footstepTimer = footStepThreshold * 0.1f;
            }
        }
    }
}
