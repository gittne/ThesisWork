using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    protected NavMeshAgent agent;

    protected bool isChasingElectricity;

    [SerializeField] float defaultSpeed;
    [SerializeField] float angerSpeed;
    [SerializeField] float disableElectricitySpeed;

    public bool IsChasingElectricity { get { return  isChasingElectricity; } }

    enum EnemyState { WANDERING, STALKING, ELECTRICITYHUNTING, CHASING }
    EnemyState enemyState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindNewDestination();
    }

    void FindNewDestination()
    {
        if (isChasingElectricity)
            return;

        agent.SetDestination(RandomNavmeshLocation(50));
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void GoChaseElectricity(Transform other)
    {
        agent.SetDestination(other.position);
        isChasingElectricity = true;
        agent.speed = angerSpeed;
    }

    public void GoToDisabling()
    {
        StartCoroutine(Disabling());
    }

    IEnumerator Disabling()
    {
        Debug.Log("i be disabling");
        yield return new WaitForSeconds(disableElectricitySpeed);
        isChasingElectricity = false;
        FindNewDestination();
    }
}
