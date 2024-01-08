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

            float randomRotation = Random.Range(0f, 360f);

            Instantiate(prefabs[randomNumber], spawnpoints.position, new Quaternion(0, randomRotation, 0, 0));
        }
    }
}
