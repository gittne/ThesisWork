using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Hand_Data : MonoBehaviour
{
    public enum HandType { Left, Right }

    public HandType handType;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;
}
