using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject keyItem
    {
        get { return keyItemPrefab; }
        private set { keyItemPrefab = value; }
    }
}

public class SCR_Key_Item_Placement_Randomizer : MonoBehaviour
{
    //[SerializeField] Transform[] spawnpoints;
    //[SerializeField] GameObject keyItemPrefab;
    [SerializeField] List<Key_Item_Generator> keyItemGenerator;

    // Start is called before the first frame update
    void Start()
    {
        RandomizingPlacement();
    }

    void RandomizingPlacement()
    {
        foreach (Key_Item_Generator generators in keyItemGenerator)
        {
            int randomNumber = UnityEngine.Random.Range(0, generators.prefabSpawns.Length);

            Instantiate(generators.keyItem, generators.prefabSpawns[randomNumber]);
        }
    }
}
