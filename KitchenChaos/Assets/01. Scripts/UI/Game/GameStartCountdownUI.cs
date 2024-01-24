using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI countdownText;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChangedEvent += HandleStateChanged;
        Display(false);
    }

    private void Update()
    {
        KitchenGameManager manager = KitchenGameManager.Instance;
        if(manager.GameState == KitchenGameManager.State.CountdownToStart)
            countdownText.text = manager.Timer.ToString("#");
    }

    private void HandleStateChanged(KitchenGameManager.State state)
    {
        Display(state == KitchenGameManager.State.CountdownToStart);
    }

    public void Display(bool active)
    {
        gameObject.SetActive(active);
    }
}
