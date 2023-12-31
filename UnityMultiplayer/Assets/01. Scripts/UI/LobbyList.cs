using System;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyList : MonoBehaviour
{
    [SerializeField] Transform lobbyItemParent = null;
    [SerializeField] LobbyItem lobbyItemPrefab = null;
    
    private bool isJoining = false;
    private bool isRefreshing = false;
    
    private void OnEnable()
    {
        RefreshLobby();
    }

    public async void RefreshLobby()
    {
        if(isRefreshing)
            return;
        
        isRefreshing = true;

        try {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;
            options.Filters = new List<QueryFilter>() {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT), // 가능한 슬롯이 0 보다 크냐
                new QueryFilter(QueryFilter.FieldOptions.IsLocked, "0", QueryFilter.OpOptions.EQ) // (Locked == false) 이냐
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

            foreach(Transform child in lobbyItemParent)
                Destroy(child.gameObject);

            foreach(Lobby lobby in lobbies.Results)
                Instantiate(lobbyItemPrefab, lobbyItemParent).Initialize(this, lobby);
            
        } catch(Exception err) {
            Debug.Log(err.Message);
        }


        isRefreshing = false;
    }

    public async void JoinAsync(Lobby lobby)
    {
        if(isJoining)
            return;
        
        isJoining = true;
        
        try {
            Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joinedLobby.Data["JoinCode"].Value;

            await ClientSingleton.Instance.GameManager.StartClientAsync(joinCode);
        } catch(Exception err) {
            Debug.Log(err.Message);
        }

        isJoining = false;
    }
}
