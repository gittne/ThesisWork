using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Light_Indicator : MonoBehaviour
{
    enum LockType { Key, Fuse }
    [SerializeField] LockType lockType;
    [SerializeField] Light lightIndicator;
    SCR_Key_Card_Reader keyReader;
    SCR_FuseBox fuseBox;

    // Start is called before the first frame update
    void Start()
    {
        lightIndicator.color = Color.red;

        if (lockType == LockType.Key)
        {
            keyReader = GetComponent<SCR_Key_Card_Reader>();
        }

        if (lockType == LockType.Fuse)
        {
            fuseBox = GetComponent<SCR_FuseBox>();
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
    }

    void KeyLight()
    {
        if (keyReader.isActivated)
        {
            lightIndicator.color = Color.green;
        }
    }

    void FuseLight()
    {
        if (fuseBox.isActivated)
        {
            lightIndicator.color = Color.green;
        }
    }
}
