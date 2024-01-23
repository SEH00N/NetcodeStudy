using System;
using UnityEngine;
using UnityEngine.UI;

public class TCharacterSelectUI : MonoBehaviour
{
	[SerializeField] Button readyButton;

    private void Awake()
    {
        readyButton.onClick.AddListener(HandleReady);
    }

    private void HandleReady()
    {
        CharacterSelectReady.Instance.SetPlayerReady();
    }
}
