using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_ActivateTeleportRay : MonoBehaviour
{
    [SerializeField] InputActionProperty activate;
    [SerializeField] LineRenderer teleportationLine;

    void Update()
    {
        teleportationLine.enabled = activate.action.ReadValue<float>() > 0.1f;
    }
}
