using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private const string AUDIO_VOLUME_KEY = "AudioVolume";

    public static AudioManager Instance;

    [SerializeField] AudioAssetsSO audioAssets;

    private float volume = 1f;
    public float Volume => volume;

    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(AUDIO_VOLUME_KEY, 0.5f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnDeliveredEvent += HandleDelivered;
        CuttingCounter.OnAnyCutEvent += HandleCutting;
        BaseCounter.OnAnyPlacedEvent += HandlePlaced;
        TrashCounter.OnAnyTrashedEvent += HandleTrash;
        DEFINE.Player.OnPickSomethingEvent += HandlePicking;
    }

    public void PlayAudio(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * this.volume);
    }

    public void PlayAudio(AudioClip[] clips, Vector3 position, float volume = 1f)
    {
        PlayAudio(clips.PickRandom(), position, volume);
    }

    private void HandleTrash(TrashCounter counter)
    {
        PlayAudio(audioAssets.trash, counter.transform.position, 1f);
    }

    private void HandlePlaced(BaseCounter counter, KitchenObject kitchenObject)
    {
        if (kitchenObject == null)
            return;

        PlayAudio(audioAssets.objectDrop, counter.transform.position, 1f);
    }

    private void HandlePicking(KitchenObject kitchenObject)
    {
        if(kitchenObject == null)
            return;

        PlayAudio(audioAssets.objectPickup, DEFINE.Player.transform.position, 1f);
    }

    private void HandleCutting(CuttingCounter counter)
    {
        PlayAudio(audioAssets.chop, counter.transform.position, 1f);
    }

    private void HandleDelivered(DeliveryCounter counter, bool success)
    {
        AudioClip[] clips = success ? audioAssets.deliverySuccess : audioAssets.deliveryFail;
        PlayAudio(clips, counter.transform.position, 1f);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        volume %= 1.1f;

        PlayerPrefs.SetFloat(AUDIO_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }
}
