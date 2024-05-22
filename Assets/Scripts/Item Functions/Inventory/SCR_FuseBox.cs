using System.Collections;
using System.Collections.Generic;
//using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(SCR_Light_Indicator))]
public class SCR_FuseBox : MonoBehaviour
{
    public int fusesInserted { get; private set; }
    public bool canInsertFuse { get; private set; }
    public bool isActivated { get; private set; }
    [Range(1, 3)] [SerializeField] int fusesLeftToInsert;
    [SerializeField] GameObject[] fuseObjects;
    [SerializeField] Light[] lights;
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    void Start()
    {
        isActivated = false;

        for (int i = 0; i < fusesLeftToInsert; i++)
        {
            fuseObjects[i].SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player has entered my fusebox zone");
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
        Debug.Log("My fusebox was filled.");

        //if (SCR_MultiplayerOverlord.Instance != null)
        //{
        //    FillFuseboxServerRpc();
        //    return;
        //}

        fusesInserted++;

        for (int i = 0; i < fusesInserted; i++)
        {
            fuseObjects[i].SetActive(true);
        }

        if (fusesInserted == fusesLeftToInsert)
        {
            isActivated = true;
        }

        if (isActivated && lights != null)
        {
            foreach (Light light in lights)
            {
                light.enabled = true;

                float randomPitch = Random.Range(0.85f, 1.15f);

                audioSource.pitch = randomPitch;

                audioSource.PlayOneShot(audioClip);
            }
        }
    }

    //[ServerRpc(RequireOwnership = false)]
    //public void FillFuseboxServerRpc()
    //{
    //    FillFuseboxClientRpc();
    //}

    //[ClientRpc]
    //void FillFuseboxClientRpc()
    //{
    //    fusesInserted++;

    //    for (int i = 0; i < fusesInserted; i++)
    //    {
    //        fuseObjects[i].SetActive(true);
    //    }

    //    if (fusesInserted == fusesLeftToInsert)
    //    {
    //        isActivated = true;
    //    }
    //}
}
