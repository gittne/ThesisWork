using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FuseBox : MonoBehaviour
{
    int fusesInserted;
    public bool canInsertFuse { get; private set; }
    public bool isActivated { get; private set; }
    [SerializeField] int fusesLeftToInsert;
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
            canInsertFuse = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInsertFuse = false;
        }
    }

    public void FillFusebox()
    {
        fusesInserted++;

        if (fusesInserted == fusesLeftToInsert)
        {
            lightIndicator.color = Color.green;

            isActivated = true;
        }
    }
}
