using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] int playerIndex = 0;
    [SerializeField] GameObject readyText;
    [SerializeField] PlayerVisual playerVisual;
    [SerializeField] Button kickButton;

    private void Awake()
    {
        if(NetworkManager.Singleton.IsServer)
            kickButton.onClick.AddListener(HandleKick);
        else
            kickButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChangedEvent += HandlePlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChangedEvent += HandleReadyChanged;

        UpdatePlayer();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChangedEvent -= HandlePlayerDataNetworkListChanged;
    }

    private void HandleKick()
    {
        PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataByIndex(playerIndex);
        KitchenGameMultiplayer.Instance.KickPlayer(playerData.clientID);
    }

    private void HandleReadyChanged()
    {
        UpdatePlayer();
    }

    private void HandlePlayerDataNetworkListChanged(NetworkListEvent<PlayerData> eventData)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        bool connected = KitchenGameMultiplayer.Instance.IsPlayerIndexConnected(playerIndex);
        Display(connected);

        if(connected)
        {
            PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataByIndex(playerIndex);
            bool isReady = CharacterSelectReady.Instance.IsPlayerReady(playerData.clientID);
            readyText.SetActive(isReady);

            playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.colorID));
        }
    }

    private void Display(bool active) => gameObject.SetActive(active);
}
