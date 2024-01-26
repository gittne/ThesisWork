using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Door_Unlock_VR : MonoBehaviour
{
    enum lockType { Key, Fuses }
    [SerializeField] lockType LockType;
    [SerializeField] SCR_Fusebox_VR fusebox;
    [SerializeField] SCR_Key_Card_Reader_VR keyReader;
    [SerializeField] BoxCollider lockCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LockType == lockType.Key)
        {
            KeyActivation();
        }

        if (LockType == lockType.Fuses)
        {
            FuseBoxActivation();
        }
    }

    void FuseBoxActivation()
    {
        if (fusebox.isActivated)
        {
            lockCollider.enabled = false;
        }
        else
        {
            lockCollider.enabled = true;
        }
    }

    void KeyActivation()
    {
        if (keyReader.isActivated)
        {
            lockCollider.enabled = false;
        }
        else
        {
            lockCollider.enabled = true;
        }
    }
}
