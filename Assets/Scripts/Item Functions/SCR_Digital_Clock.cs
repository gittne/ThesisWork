using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_Digital_Clock : MonoBehaviour
{
    [SerializeField] TextMeshPro clockText;
    [SerializeField] int hours;
    [SerializeField] int minutes;
    [SerializeField] float secondsElapsed;

    // Update is called once per frame
    void Update()
    {
        secondsElapsed += Time.deltaTime;
        minutes = Mathf.FloorToInt(secondsElapsed / 60);

        if (minutes >= 60)
        {
            hours += 1;
            secondsElapsed = 0;

            if (hours >= 24)
            {
                hours = 0;
            }
        }

        clockText.text = hours.ToString().PadLeft(2, '0') + ":" + minutes.ToString().PadLeft(2, '0');
    }
}
