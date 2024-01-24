using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectUI : MonoBehaviour
{
	[SerializeField] Button playAgainButton;

    private void Start()
    {
        playAgainButton.onClick.AddListener(HandlePlayAgain);
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        Display(false);
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }

    private void HandlePlayAgain()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.Load(Loader.Scene.MenuScene);
    }

    private void HandleClientDisconnect(ulong clientID)
    {
        if(clientID == NetworkManager.ServerClientId)
        {
            Display(true);
        }
    }

    private void Display(bool active)
    {
        gameObject.SetActive(active);
    }
}
