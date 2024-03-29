using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelectReady : NetworkBehaviour
{
    public static CharacterSelectReady Instance;

    public event Action OnReadyChangedEvent;
    private Dictionary<ulong, bool> playerReadies;
	
    private void Awake()
    {
        Instance = this;
        playerReadies = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadies[serverRpcParams.Receive.SenderClientId] = true;
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        bool allClientsReady = true;
        foreach(ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if((playerReadies.ContainsKey(clientID) == false) || (playerReadies[clientID] == false))
            {
                allClientsReady = false;
                break;
            }
        }

        if(allClientsReady)
        {
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientID)
    {
        playerReadies[clientID] = true;
        OnReadyChangedEvent?.Invoke();
    }

    public bool IsPlayerReady(ulong clientID) => (playerReadies.ContainsKey(clientID) && playerReadies[clientID]);
}
