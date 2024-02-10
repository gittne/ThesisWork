using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunnyTestThingy : MonoBehaviour
{
    Light Lightsource;

    [SerializeField] float BatteryDuration;
    [SerializeField] float MaxLightValue;
    [SerializeField] float MinLightValue;
    float timeElapsed;

    private void Start()
    {
        Lightsource = GetComponent<Light>();
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        
        if(timeElapsed < BatteryDuration)
        {
            Lightsource.intensity = Mathf.SmoothStep(MaxLightValue, MinLightValue, timeElapsed / BatteryDuration);
            Debug.Log("Time is " + timeElapsed / BatteryDuration);
        }
        



        if (Input.GetKeyDown(KeyCode.I))
        {
            timeElapsed = 0; // resets timeElapsed
        }
    }

}
