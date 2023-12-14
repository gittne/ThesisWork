using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using VivoxUnity;

public class MultiplayerOverlord : NetworkBehaviour
{
    [SerializeField] List <GameObject> players = new List <GameObject> ();



    bool deathGrace;

    public static MultiplayerOverlord Instance { get; private set; }
    private void Awake() { Instance = this; }



    private void Start()
    {
        StartCoroutine(SetupDelay());
    }

    IEnumerator SetupDelay()
    {
        while (GameObject.FindGameObjectsWithTag("Player").Length < 2)
        {
            yield return null;
        }

        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Debug.Log("added a player");
            players.Add(player);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DieDeathServerRpc()
    {
        int numberofplayerskilled = 0;
        foreach (GameObject player in players)
        {
            //ClientNetworkTransform cnt = player.GetComponent<ClientNetworkTransform>();
            //cnt.Interpolate = false;
            //cnt.Teleport(new Vector3(0, 1, 0), Quaternion.identity, transform.localScale);
            //cnt.Interpolate = true;

            //numberofplayerskilled++;

            SCR_First_Person_Controller cntr = player.GetComponent<SCR_First_Person_Controller>();
            cntr.PlayerDeathClientRpc();
        }

        Debug.Log("I killed thjis many pklayers: " + numberofplayerskilled);
    }
}
