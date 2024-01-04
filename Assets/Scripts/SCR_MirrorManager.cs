using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SCR_MirrorManager : MonoBehaviour
{
    List<GameObject> mirrors = new List<GameObject>();
    List<GameObject> players = new List<GameObject>();

    public List<GameObject> Mirrors { get { return mirrors; } }
    public List<GameObject> Players { get {  return players; } }

    void Start()
    {
        foreach(GameObject mirror in GameObject.FindGameObjectsWithTag("Mirror"))
        {
            mirrors.Add(mirror);
        }

        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(p);
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

        Debug.DrawLine(origin, mirrors[chosenIndex].transform.position, Color.white, 10);

        return mirrors[chosenIndex].transform.position;
    }

}
