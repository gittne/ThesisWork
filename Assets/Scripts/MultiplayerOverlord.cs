using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using VivoxUnity;

public class MultiplayerOverlord : NetworkBehaviour
{
    [SerializeField] List<GameObject> players = new List<GameObject>();

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

        foreach(NetworkClient player in NetworkManager.Singleton.ConnectedClientsList)
        {
            Debug.Log("added a player");
            players.Add(player.PlayerObject.gameObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DieDeathServerRpc()
    {
        foreach (GameObject player in players)
        {
            SCR_First_Person_Controller cntr = player.GetComponent<SCR_First_Person_Controller>();
            cntr.PlayerDeathClientRpc();
        }
    }
}
