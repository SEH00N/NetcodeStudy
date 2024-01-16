using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] List<KitchenObjectSO> validObjects;

    private List<KitchenObjectSO> objects;
    public List<KitchenObjectSO> Objects => objects;
    
    public event Action<KitchenObjectSO> OnIngredientAddedEvent;

    private void Awake()
    {
        objects = new List<KitchenObjectSO>();
    }

	public bool TryAddIngredient(KitchenObjectSO kitchenObject)
    {
        bool result = validObjects.Contains(kitchenObject);
        result &= !objects.Contains(kitchenObject);

        if(result)
        {
            objects.Add(kitchenObject);
            OnIngredientAddedEvent?.Invoke(kitchenObject);
        }

        return result;
    }
}
