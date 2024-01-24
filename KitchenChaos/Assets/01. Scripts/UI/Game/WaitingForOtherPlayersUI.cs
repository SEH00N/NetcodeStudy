using System;
using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.Instance.OnLocalPlayerReadyChangedEvent += HandleReady;
        KitchenGameManager.Instance.OnStateChangedEvent += HandleStateChanged;
        // Display(false);
    }

    private void HandleReady()
    {
        if(KitchenGameManager.Instance.IsLocalPlayerReady)
            Display(true);
    }

    private void HandleStateChanged(KitchenGameManager.State state)
    {
        if(state == KitchenGameManager.State.CountdownToStart)
        {
            Display(false);
        }
    }

    private void Display(bool active) => gameObject.SetActive(active);
}
