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
    [SerializeField] List<Key_Item_Generator> keyItemGenerator;

    // Start is called before the first frame update
    void Start()
    {
        if (SCR_MultiplayerOverlord.Instance == null)
        {
            RandomizingPlacement();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsHost) return;

        RandomizePlacementServerRpc();
    }

    void RandomizingPlacement()
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
