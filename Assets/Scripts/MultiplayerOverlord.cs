using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VivoxUnity;

public class MultiplayerOverlord : MonoBehaviour
{
    List <GameObject> players = new List <GameObject> ();





    public static MultiplayerOverlord Instance { get; private set; }
    private void Awake() { Instance = this; }



    private void Start()
    {
        StartCoroutine(SetupDelay());
    }

    IEnumerator SetupDelay()
    {
        while (VivoxPlayer.Instance.LoginState != LoginState.LoggedIn)
        {
            yield return null;
        }

        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }
    }






    public void PlayerDeath()
    {
        foreach (GameObject player in players)
            player.transform.position = new Vector3(0, 1, 0);
    }
}
