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
        KitchenGameMultiplayer.Instance.StartHost();
        Display(false);
    }

    private void HandleClientClicked()
    {
        KitchenGameMultiplayer.Instance.StartClient();
        Display(false);
    }

    public void Display(bool active)
    {
        gameObject.SetActive(active);
    }
}
