using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    private Allocation allocation;
    private string joinCode;
    private string lobbyID;
 
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

        try {
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>() {
                ["JoinCode"] = new DataObject(DataObject.VisibilityOptions.Member, joinCode)
            };

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync("My Lobby", MaxConnection, lobbyOptions);
            lobbyID = lobby.Id;
        } catch(Exception err) {
            Debug.Log(err.Message);
        }

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);

        HostSingleton.Instance.StartCoroutine(HeartBeatLobby(15f));
    }

    private IEnumerator HeartBeatLobby(float beating)
    {
        CustomYieldInstruction delay = new WaitForSecondsRealtime(beating);
        while(true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyID);
            yield return delay;
        }
    }
}
