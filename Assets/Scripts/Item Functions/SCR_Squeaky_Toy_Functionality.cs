using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SCR_Squeaky_Toy_Functionality : MonoBehaviour
{
    [SerializeField] GameObject toyPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject toyVisualObject;
    [SerializeField] GameObject itemHolder;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] string animationTrigger;
    public bool isHolding { get; private set; }
    [Header("Throwing Variables")]
    [SerializeField] float timeToWindUpThrow;
    [SerializeField] float timeToBringArmDown;
    [SerializeField] float initialVelocity;
    [SerializeField] float initialRotationVelocity;
    bool hasThrown;

    // Start is called before the first frame update
    void Start()
    {
        animator.enabled = false;
        toyVisualObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && isHolding && !hasThrown)
        {
            animator.enabled = true;

            if (SCR_MultiplayerOverlord.Instance != null)
            {
                ThrowPlushieServerRpc();
            }
            else
            {
                StartCoroutine(ThrowingToy());
            }
        }

        if (!isHolding)
        {
            itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, new Vector3(0f, 0f, 0.3f), 3f * Time.deltaTime);
        }
        else
        {
            toyVisualObject.SetActive(true);
        }
    }

    public void BringUpToy()
    {
        isHolding = true;
    }

    public void BringDownToy()
    {
        isHolding = false;
        toyVisualObject.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    void ThrowPlushieServerRpc()
    {
        StartCoroutine(ThrowingToy());
    }

    IEnumerator ThrowingToy()
    {
        if (isHolding && !hasThrown)
        {
            animator.Play(animationTrigger, 0, 0f);

            hasThrown = true;

            yield return new WaitForSeconds(timeToWindUpThrow);

            toyVisualObject.SetActive(false);

            GameObject instantiatedObject = Instantiate(toyPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), new Quaternion(spawnPoint.rotation.x, 0f, 0f, 0f));

            Rigidbody rigidbody = instantiatedObject.GetComponent<Rigidbody>();

            Vector3 randomRotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

            rigidbody.AddForce(spawnPoint.forward * initialVelocity, ForceMode.Impulse);

            rigidbody.angularVelocity = randomRotation * initialRotationVelocity;

            isHolding = false;

            yield return new WaitForSeconds(timeToBringArmDown);

            animator.enabled = false;

            hasThrown = false;

            itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, new Vector3(0f, 0f, 0.3f), 3f * Time.deltaTime);

            instantiatedObject.GetComponent<SCR_MonsterAttractor>().BroadcastLocation();
        }
    }
}
