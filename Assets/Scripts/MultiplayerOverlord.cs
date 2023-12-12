using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using VivoxUnity;

public class MultiplayerOverlord : MonoBehaviour
{
    List <GameObject> players = new List <GameObject> ();



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
    public void PlayerDeathServerRpc()
    {
        if (deathGrace) return;

        Debug.Log("die");
        Debug.Log("Number of players: " + players.Count + " and their positions are: " + players[0].transform.position + " and " + players[1].transform.position);

        foreach (GameObject player in players)
            player.transform.position = new Vector3(0, 1, 0);

        StartCoroutine(DeathGrace());
    }

    IEnumerator DeathGrace()
    {
        deathGrace = true;
        yield return new WaitForSeconds(5);
        deathGrace = false;
    }
}
