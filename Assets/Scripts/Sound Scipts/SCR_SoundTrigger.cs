using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SoundTrigger : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] GameObject glassbottle;
    // Start is called before the first frame update
  

     private void OnTriggerEnter(Collider other)
    {
        soundSource.Play();
        Destroy(glassbottle,2f);
    }
}
