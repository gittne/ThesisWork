using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Key_Card_Reader : MonoBehaviour
{
    bool isActivated;
    public bool canReadCard { get; private set; }
    public string keycardItemID;
    [SerializeField] Light lightIndicator;

    // Start is called before the first frame update
    void Start()
    {
        lightIndicator.color = Color.red;
        isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canReadCard = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canReadCard = false;
        }
    }

    public void ReadCard()
    {
        if (!isActivated)
        {
            lightIndicator.color = Color.green;
            isActivated = true;
            Debug.Log("Used card");
        }
    }
}
