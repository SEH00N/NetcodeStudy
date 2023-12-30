using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    private Allocation allocation;
    private string joinCode;
 
    private const int MaxConnection = 20;
    private const string GameSceneName = "GameScene";

    public async Task StartHostAsync()
    {
        try {
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConnection);
        } catch(Exception err) {
            Debug.Log(err.Message);
            return;
        }

        try {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
            GUIUtility.systemCopyBuffer = joinCode;
        } catch(Exception err) {
            Debug.Log(err.Message);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }
}
