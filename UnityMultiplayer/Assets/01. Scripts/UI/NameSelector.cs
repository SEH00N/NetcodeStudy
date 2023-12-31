using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameSelector : MonoBehaviour
{
	[SerializeField] TMP_InputField nameField;
    [SerializeField] Button connectButton;

    [Space(10f)]
    [SerializeField] int minNameLength = 1;
    [SerializeField] int maxNameLength = 12;

    private const string PlayerNameKey = "PlayerName";
    private const string NetBootstrapSceneName = "NetBootstrapScene";

    private void Start()
    {
        nameField.text = PlayerPrefs.GetString(PlayerNameKey, string.Empty);
        HandleNameChanged();
    }

    public void HandleNameChanged()
    {
        connectButton.interactable = ((minNameLength <= nameField.text.Length) && (nameField.text.Length <= maxNameLength));
    }

    public void Connect()
    {
        PlayerPrefs.SetString(PlayerNameKey, nameField.text);
        SceneManager.LoadScene(NetBootstrapSceneName);
    }
}
