using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SCR_Item_Sway : NetworkBehaviour
{
    //SUMMARY: This code is responsible for making items sway when
    //holding them in hand in the game.
    //Base code provided by "Plai": https://www.youtube.com/watch?v=QIVN-T-1QBE

    [SerializeField] CharacterController controller;
    [Header("Sway Rotation Variables")]
    [SerializeField] float swaySmoothing; //The amount of smoothing the item does when going back to default state
    [SerializeField] float swayMultiplier; //The multiplier for sway amount
    [SerializeField] float xMaxRotation; //The max rotation the object can rotate on the X axis
    [SerializeField] float yMaxRotation; //The max rotation the object can rotate on the Y axis
    [Header("Sway Movement Variables")]
    [SerializeField] float lerpSpeed; //The speed at which the item lerps when moving
    [SerializeField] float movementMultiplier; //The amount to divide the mouse input for moving the gameobject
    [SerializeField] float xMinMovement; //The max distance the item can travel on the X axis to the left
    [SerializeField] float xMaxMovement; //The max distance the item can travel on the X axis to the right
    [SerializeField] float yMinMovement; //The max distance the item can travel on the Y axis to the left
    [SerializeField] float yMaxMovement; //The max distance the item can travel on the Y axis to the right

    void Awake()
    {
        if (!IsOwner)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        Sway();
    }

    //The method which is responsible for the rotation and movement of the arm when moving the mouse
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

        targetRotation.x = Mathf.Clamp(targetRotation.x, -xMaxRotation, xMaxRotation);

        targetRotation.y = Mathf.Clamp(targetRotation.y, -yMaxRotation, yMaxRotation);

        //The code which rotates the item
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmoothing * Time.deltaTime);

        // These floats take the input from the mouse when moving it
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        //The target position which the item will move
        //towards when moving the mouse
        Vector3 targetPosition = new Vector3(moveX * movementMultiplier, moveY * movementMultiplier, 0);

        //Limits how much the arm can travel when looking around on the X axis
        targetPosition.x = Mathf.Clamp(targetPosition.x, -xMinMovement, xMaxMovement);

        //Limits how much the arm can travel when looking around on the Y axis
        targetPosition.y = Mathf.Clamp(targetPosition.y, -yMinMovement, yMaxMovement);

        // Apply damping to the movement
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, lerpSpeed * Time.deltaTime);
    }
}