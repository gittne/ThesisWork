using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuisanceEmitter : MonoBehaviour
{
    [SerializeField] private int nuisanceStrength;
    
    [SerializeField] bool isEnabled;


    public int NuisanceStrength 
    { 
        get 
        { 
            if(isEnabled)
                return nuisanceStrength;
            else
                return 0;
        } 
        set 
        { 
            nuisanceStrength = value; 
        } 
    }

    public void ToggleEnable(bool enabled)
    {
        isEnabled = enabled;
    }
}
