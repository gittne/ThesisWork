using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerpfunction : MonoBehaviour
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

        if (timeElapsed < BatteryDuration)
        {
            Lightsource.intensity = Mathf.Lerp(MaxLightValue, MinLightValue, (timeElapsed / BatteryDuration) * (timeElapsed / BatteryDuration));
            Debug.Log("Time is " + timeElapsed / BatteryDuration);
        }




        if (Input.GetKeyDown(KeyCode.I))
        {
            timeElapsed = 0; // resets timeElapsed
        }
    }
}
