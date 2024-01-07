using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Key_Item_Placement_Randomizer : MonoBehaviour
{
    [SerializeField] Transform[] spawnpoints;
    [SerializeField] GameObject keyItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        RandomizingPlacement();
    }

    void RandomizingPlacement()
    {
        int randomNumber = Random.Range(0, spawnpoints.Length);

        Instantiate(keyItemPrefab, spawnpoints[randomNumber]);
    }
}
