using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrigger : MonoBehaviour
{
    [SerializeField] bool isElectrified;
    float timeLeftOfElectricity;
    [SerializeField] float electricityUptime;
    [SerializeField] float disableElectricitySpeed;

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

    public void ToggleOn()
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
            other.GetComponent<OLDENEMYSTUFF>().GoToDisabling(disableElectricitySpeed);
            StartCoroutine(DisableTimer());
        }
    }

    IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(disableElectricitySpeed);
        isElectrified = false;
    }
}
