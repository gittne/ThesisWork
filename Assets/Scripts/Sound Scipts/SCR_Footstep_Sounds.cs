using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Footstep_Sounds : MonoBehaviour
{
    [Header("Footstep Variables")]
    [SerializeField] float stepThreshold;
    [SerializeField] AudioSource audioSource = default;
    [SerializeField] AudioClip[] carpetClips = default;
    [SerializeField] AudioClip[] woodClips = default;
    [SerializeField] AudioClip[] stoneClips = default;
    float footstepTimer = 0.2f;
    float timerActivationFloat;
    float characterSpeed;
    GameObject characterObject;
    CharacterController playerController;
    Rigidbody monsterRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        characterObject = gameObject;

        playerController = characterObject.GetComponent<CharacterController>();

        monsterRigidbody = characterObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null)
        {
            characterSpeed = new Vector2(playerController.velocity.x, playerController.velocity.z).magnitude;

            if (characterSpeed > 5f)
            {
                characterSpeed = new Vector2(playerController.velocity.x, playerController.velocity.z).magnitude / 1.3f;
            }
        }

        if (monsterRigidbody != null)
        {
            characterSpeed = new Vector2(monsterRigidbody.velocity.x, monsterRigidbody.velocity.z).magnitude;
        }

        if (characterSpeed > 0)
        {
            PlayFootsteps();
        }
    }

    void PlayFootsteps()
    {
        if (characterObject == null)
        {
            return;
        }
        
        if (characterSpeed > 0.5f)
        {
            footstepTimer -= Time.deltaTime;
        }

        timerActivationFloat = stepThreshold / characterSpeed;

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
