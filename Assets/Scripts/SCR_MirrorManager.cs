using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SCR_MirrorManager : MonoBehaviour
{
    List<GameObject> mirrors = new List<GameObject>();

    public List<GameObject> Mirrors { get { return mirrors; } }

    void Start()
    {
        foreach(GameObject mirror in GameObject.FindGameObjectsWithTag("Mirror"))
        {
            mirrors.Add(mirror);
        }
    }

    public Vector3 FindClosestEntrance(Vector3 origin)
    {
        float lowestDistance = 9999;
        int chosenIndex = 0;

        for (int i = 1; i < mirrors.Count; i++)
        {
            float currentMirrorDistance = Vector3.Distance(mirrors[i].transform.position, origin);

            if (currentMirrorDistance < lowestDistance)
            {
                lowestDistance = currentMirrorDistance;
                chosenIndex = i;
            }
        }

        return mirrors[chosenIndex].transform.position;
    }

}
