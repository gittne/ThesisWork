using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Fuze_Box : MonoBehaviour
{
    int fuzesInserted;
    public bool canInsertFuze { get; private set; }
    BoxCollider insertZone;
    [SerializeField] int fuzesLeftToInsert;
    [SerializeField] Light lightIndicator;

    // Start is called before the first frame update
    void Start()
    {
        lightIndicator.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(canInsertFuze);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInsertFuze = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInsertFuze = false;
        }
    }

    public void FillFuzebox()
    {
        fuzesInserted++;

        if (fuzesInserted == fuzesLeftToInsert)
        {
            lightIndicator.color = Color.green;

            //Code that does something
        }
    }
}
