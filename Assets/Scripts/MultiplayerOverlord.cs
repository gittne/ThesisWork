using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
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
}
