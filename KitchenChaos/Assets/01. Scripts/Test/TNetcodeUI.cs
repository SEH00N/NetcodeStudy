using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TNetcodeUI : MonoBehaviour
{
	[SerializeField] Button hostButton;
	[SerializeField] Button clientButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(HandleHostClicked);
        clientButton.onClick.AddListener(HandleClientClicked);
    }

    private void HandleHostClicked()
    {
        NetworkManager.Singleton.StartHost();
        Display(false);
    }

    private void HandleClientClicked()
    {
        NetworkManager.Singleton.StartClient();
        Display(false);
    }

    public void Display(bool active)
    {
        gameObject.SetActive(active);
    }
}
