using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrigger : MonoBehaviour
{
    [SerializeField] bool isElectrified;
    float timeLeftOfElectricity;
    [SerializeField] float electricityUptime;

    public bool IsElectrified { get { return isElectrified; } }

    private void Update()
    {
        if(timeLeftOfElectricity > 0)
        {
            timeLeftOfElectricity -= Time.deltaTime;
        }
        else
        {
            timeLeftOfElectricity = 0;
            isElectrified = false;
        }

        if(Input.GetKeyDown(KeyCode.E)) 
        {
            ToggleOn();
        }
    }

    void ToggleOn()
    {
        isElectrified = true;
        timeLeftOfElectricity = electricityUptime;
    }

    public void ResetElectricityTimer()
    {
        timeLeftOfElectricity = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("gon do styuff xd");
            other.GetComponent<EnemyMovement>().GoToDisabling();
        }
        else
        {
            Debug.Log("nah not an enemy");
        }
    }
}
