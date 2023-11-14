using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    protected NavMeshAgent agent;

    protected bool isChasingElectricity;

    [SerializeField] float defaultSpeed;
    [SerializeField] float defaultAcceleration;

    [SerializeField] float angerSpeed;
    [SerializeField] float angerAcceleration;

    public bool IsChasingElectricity { get { return  isChasingElectricity; } }

    enum EnemyState { IDLE, WANDERING, STALKING, ELECTRICITYHUNTING, CHASING }
    EnemyState enemyState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindNewDestination();
        InvokeRepeating("DestinationPoint", 0, 0.25f);
    }
    void FindNewDestination()
    {
        if (isChasingElectricity)
            return;

        agent.SetDestination(RandomNavmeshLocation(100));
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
        agent.acceleration = angerAcceleration;
    }

    public void GoToDisabling(float time)
    {
        StartCoroutine(Disabling(time));
    }

    IEnumerator Disabling(float time)
    {
        agent.destination = transform.position;
        yield return new WaitForSeconds(time);
        isChasingElectricity = false;

        agent.speed = defaultSpeed;
        agent.acceleration = defaultAcceleration;

        FindNewDestination();
    }

    void DestinationPoint()
    {
        Debug.DrawLine(transform.position, agent.destination, Color.blue, 0.25f);
    }
}
