using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI recipesText;
    
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChangedEvent += HandleStateChanged;
        Display(false);
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
