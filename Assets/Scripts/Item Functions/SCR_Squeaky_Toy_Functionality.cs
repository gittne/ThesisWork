using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Squeaky_Toy_Functionality : MonoBehaviour
{
    [SerializeField] GameObject toyPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject toyVisualObject;
    [SerializeField] GameObject itemHolder;
    [SerializeField] CharacterController controller;
    public bool isHolding { get; private set; }
    [Header("Throwing Variables")]
    [SerializeField] float timeToWindUpThrow;
    [SerializeField] float initialVelocity;
    [SerializeField] float initialRotationVelocity;
    float forwardSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(ThrowingToy());
        }

        if (isHolding)
        {
            toyVisualObject.SetActive(true);
        }
        else
        {
            toyVisualObject.SetActive(false);
            itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, new Vector3(0f, 0f, 0.3f), 3f * Time.deltaTime);
        }

        forwardSpeed = controller.transform.InverseTransformDirection(controller.velocity).z;

        Debug.Log("Speed of character is: " + forwardSpeed);
    }

    public void BringUpToy()
    {
        isHolding = true;
        Debug.Log("Bringing up toy");
    }

    IEnumerator ThrowingToy()
    {
        if (isHolding)
        {
            //Play animation of throwing toy

            yield return new WaitForSeconds(timeToWindUpThrow);

            GameObject instantiatedObject = Instantiate(toyPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), new Quaternion(spawnPoint.rotation.x, 0f, 0f, 0f));

            Rigidbody rigidbody = instantiatedObject.GetComponent<Rigidbody>();

            Vector3 randomRotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

            rigidbody.AddForce(spawnPoint.forward * initialVelocity + new Vector3(0, 0, forwardSpeed), ForceMode.Impulse);
            rigidbody.angularVelocity = randomRotation * initialRotationVelocity;

            isHolding = false;
        }
    }
}
