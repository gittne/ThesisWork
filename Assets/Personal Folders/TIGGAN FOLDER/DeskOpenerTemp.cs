using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskOpenerTemp : MonoBehaviour
{
    bool isOpen;
    bool isChanging;
    [SerializeField] Animator animator;
    float openSpeed;

    // Start is called before the first frame update
    void Start()
    {

        isOpen = true;
        animator = GetComponent<Animator>();
        openSpeed = animator.speed;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(ChangeState());
        }
    }


    IEnumerator ChangeState()
    {
        animator.SetBool("Open", isOpen);

        yield return new WaitForSeconds(openSpeed);

        isOpen = !isOpen;
    }
}
