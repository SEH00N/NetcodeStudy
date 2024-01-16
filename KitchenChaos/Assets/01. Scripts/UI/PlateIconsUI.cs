using System;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] PlateIconsSingleUI iconPrefab;
	private PlateKitchenObject plate;

    private void Awake()
    {
        plate = transform.parent.GetComponent<PlateKitchenObject>();
    }

    private void Start()
    {
        plate.OnIngredientAddedEvent += HandleIngredientAdded;
    }

    private void HandleIngredientAdded(KitchenObjectSO objectData)
    {
        UpdateVisual(objectData);
    }

    private void UpdateVisual(KitchenObjectSO objectData)
    {
        PlateIconsSingleUI icon = Instantiate(iconPrefab, transform);
        icon.SetKitchenObjetData(objectData);
    }
}
