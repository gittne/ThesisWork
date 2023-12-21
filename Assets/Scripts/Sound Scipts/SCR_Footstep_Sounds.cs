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
    [SerializeField] float footstepTimer;
    float timerActivationFloat;
    float footStepThreshold;
    [SerializeField] CharacterController characterObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        footStepThreshold = new Vector2(characterObject.velocity.x, characterObject.velocity.z).magnitude;
        if (footStepThreshold > 0)
        {
            PlayFootsteps();
        }
        Debug.Log(footstepTimer);
    }

    void PlayFootsteps()
    {
        if (characterObject == null)
        {
            return;
        }

        if (footStepThreshold > 0)
        {
            footstepTimer -= Time.deltaTime;
        }

        if (footStepThreshold > 0.001f && characterObject.velocity.y == 0)
        {
            timerActivationFloat = stepSpeed / footStepThreshold;
        }

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(characterObject.transform.position, Vector3.down, out RaycastHit hit, 3))
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

            footstepTimer = timerActivationFloat;
        }
    }
}
