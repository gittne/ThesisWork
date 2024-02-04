using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Key_Item_Generator
{
    [SerializeField] Transform[] spawnpoints;
    public Transform[] prefabSpawns
    {
        get { return spawnpoints; }
        private set { spawnpoints = value; }
    }
    [SerializeField] GameObject keyItemPrefab;
    [SerializeField] GameObject keyItemPrefab_Multiplayer;
    public GameObject keyItem { get { return keyItemPrefab; } private set { keyItem = value; } }
    public GameObject keyItem_Multiplayer{ get { return keyItemPrefab_Multiplayer; } private set { keyItem_Multiplayer = value; } }
}

public class SCR_Key_Item_Placement_Randomizer : NetworkBehaviour
{
    public static SCR_Key_Item_Placement_Randomizer Instance;
    private void Awake() { Instance = this; }

    [SerializeField] List<Key_Item_Generator> keyItemGenerator;

    public override void OnNetworkSpawn()
    {
        if (!IsHost) return;

        RandomizePlacementServerRpc();
    }

    public void RandomizePlacements()
    {
        foreach (Key_Item_Generator generators in keyItemGenerator)
        {
            int randomNumber = UnityEngine.Random.Range(0, generators.prefabSpawns.Length);

            Instantiate(generators.keyItem, generators.prefabSpawns[randomNumber]);
        }
    }

    [ServerRpc(RequireOwnership = true)]
    void RandomizePlacementServerRpc()
    {
        foreach (Key_Item_Generator generators in keyItemGenerator)
        {
            int randomNumber = UnityEngine.Random.Range(0, generators.prefabSpawns.Length);

            GameObject temp = Instantiate(generators.keyItem_Multiplayer, generators.prefabSpawns[randomNumber]);
            temp.GetComponent<NetworkObject>().Spawn();
        }
    }
}
