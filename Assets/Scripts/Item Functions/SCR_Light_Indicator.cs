using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Light_Indicator : MonoBehaviour
{
    enum LockType { Key, Fuse, KeyVR, FuseVR }
    [SerializeField] LockType lockType;
    [SerializeField] GameObject lightIndicator;
    Material material;
    SCR_Key_Card_Reader keyReader;
    SCR_FuseBox fuseBox;
    SCR_Fusebox_VR fuseBoxVR;

    // Start is called before the first frame update
    void Start()
    {
        material = lightIndicator.GetComponent<Renderer>().material;

        if (lockType == LockType.Key)
        {
            keyReader = GetComponent<SCR_Key_Card_Reader>();
        }

        if (lockType == LockType.Fuse)
        {
            fuseBox = GetComponent<SCR_FuseBox>();
        }

        if (lockType == LockType.FuseVR)
        {
            fuseBoxVR = GetComponent<SCR_Fusebox_VR>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lockType == LockType.Key)
        {
            KeyLight();
        }

        if (lockType == LockType.Fuse)
        {
            FuseLight();
        }

        if (lockType == LockType.FuseVR)
        {
            FuseLightVR();
        }
    }

    void KeyLight()
    {
        if (keyReader.isActivated)
        {
            material.SetColor("_EmissionColor", Color.green);
        }
        else if (!keyReader.isActivated && keyReader.canActivate)
        {
            material.SetColor("_EmissionColor", Color.red);
        }
        else
        {
            material.SetColor("_EmissionColor", Color.black);
        }
    }

    void FuseLight()
    {
        if (fuseBox.isActivated)
        {
            material.SetColor("_EmissionColor", Color.green);
        }
        else
        {
            material.SetColor("_EmissionColor", Color.red);
        }
    }

    void FuseLightVR()
    {
        if (fuseBoxVR.isActivated)
        {
            material.SetColor("_EmissionColor", Color.green);
        }
        else
        {
            material.SetColor("_EmissionColor", Color.red);
        }
    }
}
