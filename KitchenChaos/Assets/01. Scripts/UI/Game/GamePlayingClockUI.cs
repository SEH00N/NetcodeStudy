using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
	[SerializeField] Image timerImage;

    private void Update()
    {
        KitchenGameManager kitchen = KitchenGameManager.Instance;
        if(kitchen.GamePlaying)
            timerImage.fillAmount = kitchen.GamePlayingTimeNormalized;
    }
}
