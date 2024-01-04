using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Key_Card_Reader : MonoBehaviour
{
    public bool isActivated { get; private set; }
    public bool canReadCard { get; private set; }
    public string keycardItemID;

    // Start is called before the first frame update
    void Start()
    {
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
            isActivated = true;
        }
    }
}
