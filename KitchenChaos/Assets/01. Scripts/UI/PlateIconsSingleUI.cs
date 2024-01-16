using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingleUI : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = transform.Find("Icon").GetComponent<Image>();
    }

	public void SetKitchenObjetData(KitchenObjectSO objectData)
    {
        image.sprite = objectData.sprite;
    }
}
