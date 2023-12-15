using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using VivoxUnity;

public class MultiplayerOverlord : NetworkBehaviour
{
    [SerializeField] List <NetworkObject> players = new List <NetworkObject> ();



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

        foreach(KeyValuePair<ulong, NetworkClient> player in NetworkManager.Singleton.ConnectedClients)
        {
            Debug.Log("added a player");
            players.Add(player.Value.PlayerObject);
        }
    }

    public void KillAllPlayers()
    {
        Debug.Log("playas " + players.Count);
        foreach (NetworkObject player in players)
        {
            SCR_First_Person_Controller cntr = player.gameObject.GetComponent<SCR_First_Person_Controller>();
            cntr.PlayerDeath();
        }
    }
}
