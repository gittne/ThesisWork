using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using VivoxUnity;

public class SCR_Item_Placement_Randomizer : NetworkBehaviour
{
    [SerializeField] Transform[] spawnpoints;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] GameObject[] multiplayerPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("START!");

        if (SCR_MultiplayerOverlord.Instance == null)
        {
            RandomizePlacements();
        }
    }

    public override void OnNetworkSpawn()
    {
        RandomizePlacementsServerRpc();
    }


    void RandomizePlacements()
    {
        foreach (Transform spawnpoints in spawnpoints)
        {
            int randomNumber = Random.Range(0, prefabs.Length);

            float randomRotation = Random.Range(0f, 360f);

            Instantiate(prefabs[randomNumber], spawnpoints.position, new Quaternion(0, randomRotation, 0, 0));
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void RandomizePlacementsServerRpc()
    {
        Debug.Log("Spawning multiplayer prefabs");
        foreach (Transform spawnpoints in spawnpoints)
        {
            int randomNumber = Random.Range(0, multiplayerPrefabs.Length);

            float randomRotation = Random.Range(0f, 360f);

            Instantiate(multiplayerPrefabs[randomNumber], spawnpoints.position, new Quaternion(0, randomRotation, 0, 0));
        }
    }
}
