using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Item_Sway_Singleplayer : MonoBehaviour
{
    //SUMMARY: This code is responsible for making items sway when
    //holding them in hand in the game.
    //Base code provided by "Plai": https://www.youtube.com/watch?v=QIVN-T-1QBE

    [SerializeField] SCR_Inventory_Visual inventory;
    float mouseX;
    float mouseY;
    [Header("Sway Rotation Variables")]
    [SerializeField] float swaySmoothing; //The amount of smoothing the item does when going back to default state
    [SerializeField] float swayMultiplier; //The multiplier for sway amount
    [SerializeField] float xMaxRotation; //The max rotation the object can rotate on the X axis
    [SerializeField] float yMaxRotation; //The max rotation the object can rotate on the Y axis
    [SerializeField] Quaternion inventoryRotation;
    Quaternion targetRotation;
    [Header("Sway Movement Variables")]
    [SerializeField] float lerpSpeed; //The speed at which the item lerps when moving
    [SerializeField] float movementMultiplier; //The amount to divide the mouse input for moving the gameobject
    [SerializeField] float xMinMovement; //The max distance the item can travel on the X axis to the left
    [SerializeField] float xMaxMovement; //The max distance the item can travel on the X axis to the right
    [SerializeField] float yMinMovement; //The max distance the item can travel on the Y axis to the left
    [SerializeField] float yMaxMovement; //The max distance the item can travel on the Y axis to the right
    [SerializeField] Vector3 inventoryPosition;
    Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        //These floats take the input from the mouse when moving it
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        Sway();
    }

    //The method which is responsible for the rotation and movement of the arm when moving the mouse
    void Sway()
    {
        if (!inventory.isInventoryActive)
        {
            //The calculations for the target rotation on the X and Y axis
            Quaternion xRotation = Quaternion.AngleAxis(-mouseY * swayMultiplier, Vector3.right);
            Quaternion yRotation = Quaternion.AngleAxis(mouseX * swayMultiplier, Vector3.up);

            //The target rotation, which is the X and Y rotation multiplied
            targetRotation = xRotation * yRotation;

            targetRotation.x = Mathf.Clamp(targetRotation.x, -xMaxRotation, xMaxRotation);

            targetRotation.y = Mathf.Clamp(targetRotation.y, -yMaxRotation, yMaxRotation);

            //The target position which the item will move
            //towards when moving the mouse
            targetPosition = new Vector3(mouseX * movementMultiplier, mouseY * movementMultiplier, 0);

            //Limits how much the arm can travel when looking around on the X axis
            targetPosition.x = Mathf.Clamp(targetPosition.x, -xMinMovement, xMaxMovement);

            //Limits how much the arm can travel when looking around on the Y axis
            targetPosition.y = Mathf.Clamp(targetPosition.y, -yMinMovement, yMaxMovement);

            //The code which rotates the item
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmoothing * Time.deltaTime);

            // Apply damping to the movement
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, lerpSpeed * Time.deltaTime);
        }
        else
        {
            //The code which rotates the item
            transform.localRotation = Quaternion.Slerp(transform.localRotation, inventoryRotation, swaySmoothing * Time.deltaTime);

            // Apply damping to the movement
            transform.localPosition = Vector3.Lerp(transform.localPosition, inventoryPosition, lerpSpeed * 2f * Time.deltaTime);
        }
    }
}
