using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string MUSIC_VOLUME_KEY = "MusicVolume";

    public static MusicManager Instance;

    private AudioSource audioSource;

    private float volume = 0.3f;
    public float Volume => volume;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.5f);
    }

    private void Start()
    {
        audioSource.volume = volume;
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        volume %= 1.1f;

        audioSource.volume = volume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }
}
