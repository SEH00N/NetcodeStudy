using System;
using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.Instance.OnGlobalGamePausedEvent += HandleGlobalGamePaused;
        Display(false);
    }

    private void HandleGlobalGamePaused(bool isPaused)
    {
        Display(isPaused);
    }

    private void Display(bool active) => gameObject.SetActive(active);
}
