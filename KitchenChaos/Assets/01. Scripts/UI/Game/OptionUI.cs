using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] InputReaderSO inputReader;

    [Space(10f)]
	[SerializeField] Button soundEffectButton;
    [SerializeField] TextMeshProUGUI soundEffectText;

    [Space(10f)]
	[SerializeField] Button musicButton;
    [SerializeField] TextMeshProUGUI musicText;

    [Space(10f)]
    [SerializeField] Button closeButton;

    [Space(10f)]
    [SerializeField] GameObject rebindBlockPanel;

    private void Awake()
    {
        soundEffectButton.onClick.AddListener(HandleSoundEffect);
        musicButton.onClick.AddListener(HandleMusic);
        closeButton.onClick.AddListener(HandleClose);
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnLocalGamePausedEvent += HandlePaused;

        UpdateVisual();
        Display(false);
        DisplayRebindBlockPanel(false);
    }

    private void HandlePaused(bool paused)
    {
        if(paused == false)
            Display(false);
    }

    private void HandleClose()
    {
        Display(false);
    }

    private void HandleMusic()
    {
        MusicManager.Instance.ChangeVolume();
        UpdateVisual();
    }

    private void HandleSoundEffect()
    {
        AudioManager.Instance.ChangeVolume();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        soundEffectText.text = $"Sound Effects : {Mathf.Round(AudioManager.Instance.Volume * 10f)}";
        musicText.text = $"Music : {Mathf.Round(MusicManager.Instance.Volume * 10f)}";
    }

    public void Display(bool active)
    {
        gameObject.SetActive(active);
    }

    public void DisplayRebindBlockPanel(bool active)
    {
        rebindBlockPanel.SetActive(active);
    }
}
