using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSlot : MonoBehaviour
{
	[SerializeField] int colorID;
    [SerializeField] Image image;
    [SerializeField] GameObject selectedObject;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChangedEvent += HandlePlayerDataNetworkListChanged;
        image.color = KitchenGameMultiplayer.Instance.GetPlayerColor(colorID);
        UpdateIsSelected();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChangedEvent -= HandlePlayerDataNetworkListChanged;
    }

    private void HandlePlayerDataNetworkListChanged(NetworkListEvent<PlayerData> eventData)
    {
        UpdateIsSelected();
    }

    private void HandleClick()
    {
        KitchenGameMultiplayer.Instance.ChangePlayerColor(colorID);
    }

    private void UpdateIsSelected()
    {
        if(KitchenGameMultiplayer.Instance.GetLocalPlayerData().colorID == colorID)
            selectedObject.SetActive(true);
        else
            selectedObject.SetActive(false);
    }
}
