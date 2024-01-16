using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] float fillingSpeed = 3f;
    [SerializeField] GameObject progressorReference;

    private IProgressable progressor;
	private Image barImage;

    private float targetFillAmount = 0f;

    private void Awake()
    {
        progressor = progressorReference.GetComponent<IProgressable>();
        barImage = transform.Find("Bar").GetComponent<Image>();
    }

    private void Start()
    {
        progressor.OnProgressChangedEvent += HandleProgress;
        barImage.fillAmount = targetFillAmount = 0f;

        Display(false);
    }

    private void Update()
    {
        barImage.fillAmount = Mathf.Lerp(barImage.fillAmount, targetFillAmount, fillingSpeed * Time.deltaTime);

        if(barImage.fillAmount <= 0f || barImage.fillAmount >= 0.99f)
            Display(false);
    }

    public void Display(bool value)
    {
        gameObject.SetActive(value);
    }

    private void HandleProgress(float current, float max, bool immediately)
    {
        targetFillAmount = current / max;

        if(immediately)
        {
            barImage.fillAmount = targetFillAmount;
            Display(0 < targetFillAmount && barImage.fillAmount < 0.99f);
        }
        else if(gameObject.activeSelf == false)
            Display(true);
    }
}
