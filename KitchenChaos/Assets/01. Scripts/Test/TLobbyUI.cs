using System;
using UnityEngine;
using UnityEngine.UI;

public class TLobbyUI : MonoBehaviour
{
	[SerializeField] Button createGameButton;
	[SerializeField] Button joinGameButton;

    private void Awake()
    {
        createGameButton.onClick.AddListener(HandleCreateGame);
        joinGameButton.onClick.AddListener(HandleJoinGame);
    }

    private void HandleCreateGame()
    {
        KitchenGameMultiplayer.Instance.StartHost();
        Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
    }

    private void HandleJoinGame()
    {
        KitchenGameMultiplayer.Instance.StartClient();
    }
}
