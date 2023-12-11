using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Digital_Clock : MonoBehaviour
{
    Text clockText;

    // Start is called before the first frame update
    void Awake()
    {
        clockText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string hours = "18";
    }

    string LeadingZero(int number)
    {
        return number.ToString().PadLeft(2, '0');
    }
}
