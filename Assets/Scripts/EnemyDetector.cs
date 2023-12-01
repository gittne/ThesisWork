using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private List<GameObject> nuisancesList;
    public List<GameObject> NuisancesList { get { return nuisancesList; } }

    EnemyBrain brain;

    private void Start()
    {
        InvokeRepeating("RageTick", 0, 1);
        brain = GetComponentInParent<EnemyBrain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NuisanceEmitter>() != null && !nuisancesList.Contains(other.gameObject))
        {
            Debug.Log("found a nuisance in my vicinity.");
            nuisancesList.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<NuisanceEmitter>() != null && nuisancesList.Contains(other.gameObject))
        {
            Debug.Log("nuisance left.");
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

        Debug.Log("I'm adding this much rage grr: " + totalNuisanceValue + " from this many sources: " + nuisancesList.Count);

        brain.AlterRage(totalNuisanceValue);
    }
}
