using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] List<PlateVisualObject> visualObjects;
    private PlateKitchenObject plate;

    private void Awake()
    {
        plate = transform.parent.GetComponent<PlateKitchenObject>();
    }

    private void Start()
    {
        plate.OnIngredientAddedEvent += HandleIngredientAdded;

        visualObjects.ForEach(i => i.gameObject.SetActive(false));
    }

    private void HandleIngredientAdded(KitchenObjectSO objectData)
    {
        DisplayVisual(objectData, true);
    }

    private void DisplayVisual(KitchenObjectSO objectData, bool value)
    {
        PlateVisualObject visual = visualObjects.Find(i => i.objectData == objectData);
        visual.gameObject.SetActive(true);
    }
}
