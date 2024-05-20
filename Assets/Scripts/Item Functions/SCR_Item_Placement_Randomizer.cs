using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using VivoxUnity;

public class SCR_Item_Placement_Randomizer : NetworkBehaviour
{
    public static SCR_Item_Placement_Randomizer Instance;
    private void Awake() { Instance = this; }

    [SerializeField] Transform[] spawnpoints;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] GameObject[] multiplayerPrefabs;

    public override void OnNetworkSpawn()
    {
        if (!IsHost) return;

        RandomizePlacementsServerRpc();
    }

    public void RandomizePlacements()
    {
        //Generates a random item for each transform in the array
        foreach (Transform spawnpoints in spawnpoints)
        {
            //Generate a random random number which corelates to an item
            int randomNumber = Random.Range(0, prefabs.Length);

            //Generate a random rotation value
            float randomRotation = Random.Range(0f, 360f);

            //Place the item on said position
            Instantiate(prefabs[randomNumber], spawnpoints.position, new Quaternion(0, randomRotation, 0, 0));
        }
    }

    [ServerRpc(RequireOwnership = true)]
    void RandomizePlacementsServerRpc()
    {
        Debug.Log("Spawning multiplayer prefabs");
        foreach (Transform spawnpoints in spawnpoints)
        {
            int randomNumber = Random.Range(0, multiplayerPrefabs.Length);

            float randomRotation = Random.Range(0f, 360f);

            GameObject temp = Instantiate(multiplayerPrefabs[randomNumber], spawnpoints.position, new Quaternion(0, randomRotation, 0, 0));
            temp.GetComponent<NetworkObject>().Spawn();
        }
    }
}
