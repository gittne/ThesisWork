using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Item_Sway : MonoBehaviour
{
    //SUMMARY: This code is responsible for making items sway when
    //holding them in hand in the game.
    //Base code provided by "Plai": https://www.youtube.com/watch?v=QIVN-T-1QBE

    [Header("Sway Rotation Variables")]
    [SerializeField] float smoothing; //The amount of smoothing the item does when going back to default state
    [SerializeField] float swayMultiplier; //The multiplier for sway amount
    [SerializeField] float xMaxRotation;
    [SerializeField] float yMaxRotation;
    [Header("Sway Movement Variables")]
    [SerializeField] float lerpSpeed; //The speed at which the item lerps when moving
    [SerializeField] float movementMultiplier; //The amount to divide the mouse input for moving the gameobject
    [SerializeField] float yMinMovement;
    [SerializeField] float yMaxMovement;
    Vector3 resetTransform;

    void Awake()
    {
        resetTransform = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Sway();
    }

    void Sway()
    {
        //These floats take the input from the mouse when moving it
        //and multiplies it with the sway multiplier float
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        //The calculations for the target rotation on the X and Y axis
        Quaternion xRotation = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(mouseX, Vector3.up);

        //The target rotation, which is the X and Y rotation multiplied
        Quaternion targetRotation = xRotation * yRotation;

        //The code which rotates the item
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothing * Time.deltaTime);

        if (transform.localRotation.x > xMaxRotation)
        {
            transform.localRotation = ClampRotation(targetRotation, xMaxRotation);
            Debug.Log("Roterade över X");
        }

        if (transform.localRotation.y > yMaxRotation)
        {
            transform.localRotation = ClampRotation(targetRotation, yMaxRotation);
            Debug.Log("Roterade över Y");
        }


        // These floats take the input from the mouse when moving it
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        //The target position which the item will move
        //towards when moving the mouse
        Vector3 targetPosition = new Vector3(moveX * movementMultiplier, moveY * movementMultiplier, 0);

        targetPosition.y = Mathf.Clamp(targetPosition.y, -yMinMovement, yMaxMovement);

        // Apply damping to the movement
        transform.localPosition = Vector3.Lerp(transform.localPosition, resetTransform + targetPosition, lerpSpeed * Time.deltaTime);
    }

    Quaternion ClampRotation(Quaternion rotation, float maxAngle)
    {
        // Extract the euler angles from the quaternion
        Vector3 eulerAngles = rotation.eulerAngles;

        // Clamp the rotation angles on both X and Y axes
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, -maxAngle, maxAngle);
        eulerAngles.y = Mathf.Clamp(eulerAngles.y, -maxAngle, maxAngle);

        // Convert the clamped euler angles back to quaternion
        return Quaternion.Euler(eulerAngles);
    }
}
