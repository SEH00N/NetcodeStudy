using System;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGameEvent += HandleTryingToJoin;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGameEvent += HandleFailedToJoin;

        Display(false);
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGameEvent -= HandleTryingToJoin;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGameEvent -= HandleFailedToJoin;
    }

    private void HandleTryingToJoin()
    {
        Display(false);
    }

    private void HandleFailedToJoin()
    {
        Display(true);
    }

    private void Display(bool active) => gameObject.SetActive(active);
}
