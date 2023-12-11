using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Threading.Tasks;

using Unity.Services.Vivox;
using VivoxUnity;

public class RelayMaker : MonoBehaviour
{
    public static RelayMaker Instance { get; private set; }
    private void Awake() { Instance = this; }

    [SerializeField] GameObject multiplayerMenu;
    [SerializeField] GameObject tempCamera;
 
    [ContextMenu("Create a Relay")]
    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            Debug.Log("Created a relay.");

            VivoxService.Instance.Initialize();
            VivoxPlayer.Instance.LoginToVivox();

            multiplayerMenu.SetActive(false);
            Destroy(tempCamera);

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();

            Debug.Log("Joined a relay.");

            VivoxService.Instance.Initialize();
            VivoxPlayer.Instance.LoginToVivox();

            multiplayerMenu.SetActive(false);
            Destroy(tempCamera);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
