using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VivoxUnity;

public class SCR_MultiplayerOverlord : NetworkBehaviour
{
    public static SCR_MultiplayerOverlord Instance { get; private set; }
    private void Awake() { Instance = this; }


    private SCR_EnemyBrain monsterBrain;
    public SCR_EnemyBrain MonsterBrain { get { return monsterBrain; } set {  monsterBrain = value; } }

    [SerializeField] List<GameObject> playerObjects;
    public List<GameObject> PlayerObjects { get { return playerObjects; } set {  playerObjects = value; } }

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

        yield return new WaitForSeconds(1);
        foreach(GameObject pla in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerObjects.Add(pla);
        }

        SwitchToMultiplayerInteractables();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerKillServerRpc()
    {
        int numberOfDeadPlayers = 0;

        foreach (GameObject player in playerObjects)
        {
            SCR_First_Person_Controller cntr = player.GetComponent<SCR_First_Person_Controller>();
            if(cntr.AmIDead.Value) numberOfDeadPlayers++;
        }

        Debug.Log("Number of dead players: " + numberOfDeadPlayers + " and number of players is: " + playerObjects.Count);

        if (numberOfDeadPlayers >= playerObjects.Count)
        {
            foreach (GameObject player in playerObjects)
            {
                SCR_First_Person_Controller cntr = player.gameObject.GetComponent<SCR_First_Person_Controller>();
                cntr.PlayerRespawnClientRpc();
            }
        }
    }

    void SwitchToMultiplayerInteractables()
    {
        foreach(GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable"))
        {
            if(interactable.GetComponentInChildren<SCR_Animated_Interactable>() != null ) interactable.GetComponentInChildren<SCR_Animated_Interactable>().enabled = false;
            if (interactable.GetComponentInChildren<SCR_Animated_Interactable_Multiplayer>() != null) interactable.GetComponentInChildren<SCR_Animated_Interactable_Multiplayer>().enabled = true;
        }
    }
}
