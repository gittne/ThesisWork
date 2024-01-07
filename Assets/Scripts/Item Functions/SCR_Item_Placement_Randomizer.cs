using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Item_Placement_Randomizer : MonoBehaviour
{
    [SerializeField] Transform[] spawnpoints;
    [SerializeField] GameObject[] prefabs;

    // Start is called before the first frame update
    void Start()
    {
        RandomizePlacements();
    }

    void RandomizePlacements()
    {
        foreach (Transform spawnpoints in spawnpoints)
        {
            int randomNumber = Random.Range(0, prefabs.Length);

            Instantiate(prefabs[randomNumber], spawnpoints);
        }
    }
}
