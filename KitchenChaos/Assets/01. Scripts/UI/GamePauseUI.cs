using System;
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

        KitchenGameManager.Instance.OnGamePausedEvent += HandlePause;
        
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
