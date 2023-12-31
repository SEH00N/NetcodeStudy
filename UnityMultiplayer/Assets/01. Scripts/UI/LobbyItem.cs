using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyItem : MonoBehaviour
{
	[SerializeField] TMP_Text lobbyNameText;
	[SerializeField] TMP_Text lobbyPlayerText;

    private LobbyList lobbyList = null;
    private Lobby lobby = null;

    public void Initialize(LobbyList lobbyList, Lobby lobby)
    {
        this.lobbyList = lobbyList;
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        lobbyPlayerText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
    }

    public void Join()
    {
        lobbyList.JoinAsync(lobby);
    }
}
