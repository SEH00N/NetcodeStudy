using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
	[SerializeField] Button playButton;
	[SerializeField] Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlay);
        quitButton.onClick.AddListener(OnQuit);

        Time.timeScale = 1;
    }

    private void OnPlay()
    {
        Loader.Load(Loader.Scene.GameScene);
    }

    private void OnQuit()
    {
        Application.Quit();
    }
}
