using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Item_Indicator : MonoBehaviour
{
    public GameObject Press_Canvas ;
   
    void OnTriggerEnter(Collider other)
    {
        Press_Canvas.SetActive(true);
    }
    void OnTriggerExit(Collider other)
    {
        Press_Canvas.SetActive(false);
    }
 //   if (true)
	//{

	//}
}
