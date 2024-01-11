using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SoundTrigger : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] GameObject soundobj;

     private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        soundSource.Play();
        Destroy(soundobj,2f);
    }
}
