using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VivoxUnity;

public class PlayerNetwork : NetworkBehaviour
{
    bool isSetUpVoice;
    private void Start()
    {
        transform.position = new Vector3(0, 1, 0);
        if (IsOwner)
        {
            StartCoroutine(SetUpVoice());
        }
    }

    IEnumerator SetUpVoice()
    {
        while(VivoxPlayer.Instance.LoginState != LoginState.LoggedIn) 
        { 
            yield return null;
        }

        VivoxPlayer.Instance.LoginSession.SetTransmissionMode(TransmissionMode.Single, VivoxPlayer.Instance.localChannel);
        InvokeRepeating("GoUpdatePosition", 0, 0.1f);
    }

    void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1.5f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1.5f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1.5f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1.5f;

        if (Input.GetKeyDown(KeyCode.Q)) EnableRadio();
        if (Input.GetKeyUp(KeyCode.Q)) DisableRadio();

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void GoUpdatePosition()
    {
        if (!VivoxPlayer.Instance.LoginSession.GetChannelSession(VivoxPlayer.Instance.localChannel).IsTransmitting)
            return;

        Update3DPosition(transform, transform);
        Debug.Log("Updating 3D position.");
    }

    void Update3DPosition(Transform listener, Transform speaker)
    {
            VivoxPlayer.Instance.TransmittingSession.Set3DPosition(speaker.position, listener.position,
                listener.forward, listener.up);
    }

    void EnableRadio()
    {
        VivoxPlayer.Instance.LoginSession.SetTransmissionMode(TransmissionMode.Single, VivoxPlayer.Instance.globalChannel);
    }

    private void DisableRadio()
    {
        VivoxPlayer.Instance.LoginSession.SetTransmissionMode(TransmissionMode.Single, VivoxPlayer.Instance.localChannel);
    }
}
