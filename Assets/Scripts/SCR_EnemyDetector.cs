using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EnemyDetector : MonoBehaviour
{
    [SerializeField] private List<GameObject> nuisancesList;
    public List<GameObject> NuisancesList { get { return nuisancesList; } }

    SCR_EnemyBrain brain;

    private void Start()
    {
        InvokeRepeating("RageTick", 0, 1);
        brain = GetComponentInParent<SCR_EnemyBrain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NuisanceEmitter>() != null && !nuisancesList.Contains(other.gameObject))
        {
            nuisancesList.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<NuisanceEmitter>() != null && nuisancesList.Contains(other.gameObject))
        {
            nuisancesList.Remove(other.gameObject);
        }
    }

    public void RageTick()
    {
        if (nuisancesList.Count == 0)
            return;

        int totalNuisanceValue = 0;

        foreach(var item in nuisancesList)
        {
            totalNuisanceValue += item.GetComponent<NuisanceEmitter>().NuisanceStrength;
        }

        brain.AlterRage(totalNuisanceValue);
    }
}
