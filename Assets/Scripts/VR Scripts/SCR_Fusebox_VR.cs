using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Fusebox_VR : MonoBehaviour
{
    public bool isActivated { get; private set; }
    [Range(1, 3)] [SerializeField] int fusesLeftToInsert;
    [SerializeField] GameObject[] fuseObjects;
    [SerializeField] GameObject[] fuseSocketsObjects;
    [SerializeField] SCR_XR_Socket_Fuse_Check[] fuseSockets;
    bool[] isFuseInserted;

    void Start()
    {
        isFuseInserted = new bool[fusesLeftToInsert];

        isActivated = false;

        foreach (GameObject fuseSockets in fuseSocketsObjects)
        {
            fuseSockets.SetActive(false);
        }

        for (int i = 0; i < fusesLeftToInsert; i++)
        {
            fuseObjects[i].SetActive(false);
            fuseSocketsObjects[i].SetActive(true);
        }
    }

    void Update()
    {
        FillFusebox();

        Debug.Log(isActivated);
    }

    public void FillFusebox()
    {
        for (int i = 0; i < isFuseInserted.Length; i++)
        {
            if (fuseSockets[i].isFuseInserted == true)
            {
                isFuseInserted[i] = true;
            }
            else
            {
                isFuseInserted[i] = false;
            }

            if (isFuseInserted[i] == false)
            {
                isActivated = false;
                return;
            }
            else
            {
                isActivated = true;
            }
        }
    }
}
