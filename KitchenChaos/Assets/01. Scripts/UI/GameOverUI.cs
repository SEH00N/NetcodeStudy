using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI recipesText;
    [SerializeField] Button playAgainButton;
    
    private void Start()
    {
        playAgainButton.onClick.AddListener(HandlePlayAgain);
        KitchenGameManager.Instance.OnStateChangedEvent += HandleStateChanged;
        
        Display(false);
    }

    private void HandlePlayAgain()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.Load(Loader.Scene.MenuScene);
    }

    private void HandleStateChanged(KitchenGameManager.State state)
    {
        bool gameOver = (state == KitchenGameManager.State.GameOver);
        if(gameOver)
        {
            recipesText.text = DeliveryManager.Instance.SucceedRecipesCount.ToString();
        }

        Display(gameOver);
    }

    public void Display(bool active)
    {
        gameObject.SetActive(active);
    }
}
