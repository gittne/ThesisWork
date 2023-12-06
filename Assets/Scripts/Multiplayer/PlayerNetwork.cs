using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VivoxUnity;

public class PlayerNetwork : NetworkBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(0, 1, 0);
        if (IsOwner) InvokeRepeating("GoUpdatePosition", 0, 1);
    }

    void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1.5f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1.5f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1.5f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1.5f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void GoUpdatePosition()
    {
        Update3DPosition(transform, transform);
    }

    void Update3DPosition(Transform listener, Transform speaker)
    {
            VivoxPlayer.Instance.TransmittingSession.Set3DPosition(speaker.position, listener.position,
                listener.forward, listener.up);
    }

}
