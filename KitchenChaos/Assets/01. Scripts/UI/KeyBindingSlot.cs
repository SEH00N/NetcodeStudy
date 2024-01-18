using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindingSlot : MonoBehaviour
{
    [SerializeField] InputReaderSO inputReader;
    [SerializeField] InputReaderSO.KeyBinding keyBinding;
    [SerializeField] int alpha = 0;

    private OptionUI optionUI;

    private TextMeshProUGUI keyText;
    private Button bindButton;


    private void Awake()
    {
        optionUI = transform.parent.parent.GetComponent<OptionUI>();

        bindButton = transform.Find("Button").GetComponent<Button>();
        keyText = bindButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        bindButton.onClick.AddListener(HandleBind);
    }

    private void Start()
    {
        keyText.text = inputReader.GetBindingText(keyBinding, alpha);
    }

    private void HandleBind()
    {
        inputReader.Rebinding(keyBinding, (keyName) => {
            optionUI.DisplayRebindBlockPanel(false);
            keyText.text = keyName;
        });
        optionUI.DisplayRebindBlockPanel(true);
    }
}
