using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button resumeButton;

    [Space(10f)]
    [SerializeField] OptionUI optionUI;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(HandleMainManu);
        resumeButton.onClick.AddListener(HandleResume);
        optionButton.onClick.AddListener(HandleOption);

        KitchenGameManager.Instance.OnLocalGamePausedEvent += HandlePause;
        
        Display(false);
    }

    private void HandleOption()
    {
        optionUI.Display(true);
    }

    private void HandleResume()
    {
        KitchenGameManager.Instance.TogglePauseGame();
    }

    private void HandleMainManu()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.Load(Loader.Scene.MenuScene);
    }

    private void HandlePause(bool paused)
    {
        Display(paused);
    }

    public void Display(bool active)
    {
        gameObject.SetActive(active);
    }
}
