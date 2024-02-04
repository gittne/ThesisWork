using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_Action_Based_Continuous_Move_Provider_Custom : MonoBehaviour
{
    [SerializeField] ActionBasedContinuousMoveProvider movementScript;

    public void CommencePlayerDeath()
    {
        StartCoroutine(DieAndRespawn());
    }

    IEnumerator DieAndRespawn()
    {
        movementScript.moveSpeed = 0;

        yield return new WaitForSeconds(2);

        //Color c = fader.color;

        //for (int i = 0; i < 51; i++)
        //{
        //    c.a = 0.05f * i;
        //    fader.color = c;

        //    yield return new WaitForSeconds(0.001f);
        //}

        transform.position = new Vector3(7, 0, 72);

        yield return new WaitForSeconds(4.5f);

        movementScript.moveSpeed = 4;

        //yield return new WaitForSeconds(0.5f);

        //for (int i = 0; i < 101; i++)
        //{
        //    c.a = 1 - 0.01f * i;
        //    fader.color = c;

        //    yield return new WaitForSeconds(0.01f);
        //}
    }
}
