using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseMessageUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(HandleClose);
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGameEvent += HandleFailedToJoin;
        Display(false);
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGameEvent -= HandleFailedToJoin;
    }

    private void HandleFailedToJoin()
    {
        Display(true);

        messageText.text = NetworkManager.Singleton.DisconnectReason;
        if(messageText.text == "")
            messageText.text = "Failed to connect";
    }

    private void HandleClose()
    {
        Display(false);
    }

    private void Display(bool active) => gameObject.SetActive(active);
}
