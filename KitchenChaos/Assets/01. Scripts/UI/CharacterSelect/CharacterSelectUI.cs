using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
	[SerializeField] Button mainMenuButton;
	[SerializeField] Button readyButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(HandleMainMenu);
        readyButton.onClick.AddListener(HandleReady);
    }

    private void HandleMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.Load(Loader.Scene.MenuScene);
    }

    private void HandleReady()
    {
        CharacterSelectReady.Instance.SetPlayerReady();
    }
}
