using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SCR_Hand_Pose_Preset : MonoBehaviour
{
    XRGrabInteractable grabInteractable;

    [Header("Poses")]
    [SerializeField] SCR_Hand_Data handPoses;

    Vector3 startingHandPosition;
    Vector3 finalHandPosition;

    Quaternion startingHandRotation;
    Quaternion finalHandRotation;
    Quaternion[] startingFingerRotations;
    Quaternion[] finalFingerRotations;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(Pose);

        //grabInteractable.selectExited.AddListener(ResetPose);

        handPoses.gameObject.SetActive(false);
    }

    void Pose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            SCR_Hand_Data poseData = arg.interactorObject.transform.GetComponentInChildren<SCR_Hand_Data>();

            poseData.animator.enabled = false;

            //SetHandValues(poseData, handPoses);

            //SetHandData(poseData, finalHandPosition, finalHandRotation, finalFingerRotations);
        }
    }

    //void ResetPose(BaseInteractionEventArgs arg)
    //{
    //    if (arg.interactorObject is XRDirectInteractor)
    //    {
    //        SCR_Hand_Data poseData = arg.interactorObject.transform.GetComponentInChildren<SCR_Hand_Data>();

    //        poseData.animator.enabled = true;

    //        SetHandData(poseData, startingHandPosition, startingHandRotation, startingFingerRotations);
    //    }
    //}

    //void SetHandValues(scr_hand_data firsthand, scr_hand_data secondhand)
    //{
    //    startinghandposition = new vector3(firsthand.root.localposition.x / firsthand.root.localscale.x, 
    //        firsthand.root.localposition.y / firsthand.root.localscale.y, 
    //        firsthand.root.localposition.z / firsthand.root.localscale.z);
    //    finalhandposition = new vector3(secondhand.root.localposition.x / secondhand.root.localscale.x,
    //        secondhand.root.localposition.y / secondhand.root.localscale.y,
    //        secondhand.root.localposition.z / secondhand.root.localscale.z);

    //    startinghandrotation = firsthand.root.localrotation;
    //    finalhandrotation = secondhand.root.localrotation;

    //    startingfingerrotations = new quaternion[firsthand.fingerbones.length];
    //    finalfingerrotations = new quaternion[secondhand.fingerbones.length];

    //    for (int i = 0; i < firsthand.fingerbones.length; i++)
    //    {
    //        startingfingerrotations[i] = firsthand.fingerbones[i].localrotation;
    //        finalfingerrotations[i] = secondhand.fingerbones[i].localrotation;
    //    }
    //}

    //void SetHandData(SCR_Hand_Data hand, Vector3 position, Quaternion rotation, Quaternion[] boneRotations)
    //{
    //    hand.root.localPosition = position;
    //    hand.root.localRotation = rotation;

    //    for (int i = 0; i < boneRotations.Length; i++)
    //    {
    //        hand.fingerBones[i].localRotation = boneRotations[i];
    //    }
    //}
}
