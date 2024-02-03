using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FlashlightIndicator : MonoBehaviour
{
    Renderer renderer;

    [SerializeField] Gradient gradient; // gradient, as in the name
    
    [SerializeField] float BatteryDuration; // How long it takes to go through Gradient
    float timeElapsed;

    void Start()
    {
        renderer = GetComponent<Renderer>(); // gets the renderer from the object
    }

    void Update()
    {
       
        timeElapsed += Time.deltaTime;
        float PercentageBattery = timeElapsed / BatteryDuration; // Divides time passed with batterylife duration giving you a vlaue between 0-1.

        renderer.material.color = gradient.Evaluate(PercentageBattery); // travers between start of gradient to end of gradient chaning material color. 
        

        if (Input.GetKeyDown(KeyCode.I))
        {
            timeElapsed = 0; // resets timeElapsed
        }
    }

        
}
