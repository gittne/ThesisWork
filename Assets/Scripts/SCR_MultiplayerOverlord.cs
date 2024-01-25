using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VivoxUnity;

public class SCR_MultiplayerOverlord : NetworkBehaviour
{
    [SerializeField] List <NetworkObject> players = new List <NetworkObject> ();
    public List<NetworkObject> Players { get { return players; } }

    public static SCR_MultiplayerOverlord Instance { get; private set; }
    private void Awake() { Instance = this; }


    private SCR_EnemyBrain monsterBrain;
    public SCR_EnemyBrain MonsterBrain { get { return monsterBrain; } set {  monsterBrain = value; } }

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

        if (GameObject.FindWithTag("Enemy") != null)
            monsterBrain = GameObject.FindWithTag("Enemy").GetComponent<SCR_EnemyBrain>();

        SwitchToMultiplayerInteractables();
    }

    [ServerRpc(RequireOwnership = false)]
    public void KillAllPlayersServerRpc()
    {
        Debug.Log("playas " + players.Count);
        foreach (NetworkObject player in players)
        {
            SCR_First_Person_Controller cntr = player.gameObject.GetComponent<SCR_First_Person_Controller>();
            cntr.PlayerDeathClientRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckPlayerHealthStatusServerRpc()
    {
        int numberOfDeadPlayers = 0;

        foreach (NetworkObject player in players)
        {
            SCR_First_Person_Controller cntr = player.gameObject.GetComponent<SCR_First_Person_Controller>();
            if(cntr.AmIDead.Value) numberOfDeadPlayers++;
        }

        Debug.Log("Number of dead players: " + numberOfDeadPlayers + " and number of players is: " + players.Count);

        if (numberOfDeadPlayers >= players.Count)
            foreach (NetworkObject player in players)
            {
                SCR_First_Person_Controller cntr = player.gameObject.GetComponent<SCR_First_Person_Controller>();
                cntr.BackToMenuClientRpc();
            }
    }

    void SwitchToMultiplayerInteractables()
    {
        foreach(GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable"))
        {
            interactable.GetComponent<SCR_Animated_Interactable>().enabled = false;
            interactable.GetComponent<SCR_Animated_Interactable_Multiplayer>().enabled = true;
        }
    }
}
