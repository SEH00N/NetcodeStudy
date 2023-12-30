using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private const string MenuSceneName = "MenuScene";
    private const string GameSceneName = "GameScene";

    private JoinAllocation allocation;

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
        
        AuthState authState = await AuthenticationWrapper.DoAuth();
        return (authState == AuthState.Authenticated);
    }

    public async Task StartClientAsync(string joinCode)
    {
        try {
            allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        } catch(Exception err) {
            Debug.Log(err.Message);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
        // NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
}
