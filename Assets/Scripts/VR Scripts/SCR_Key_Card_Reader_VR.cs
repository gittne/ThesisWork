using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Key_Card_Reader_VR : MonoBehaviour
{
    public bool isActivated;
    public bool canReadCard { get; private set; }
    public bool canActivate;
    public string keycardTag;

    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(keycardTag))
        {
            isActivated = true;
            Debug.Log("Activated keyreader");
        }
    }
}
