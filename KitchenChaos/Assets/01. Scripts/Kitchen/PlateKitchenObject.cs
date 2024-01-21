using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] List<KitchenObjectSO> validObjects;

    private List<KitchenObjectSO> objects;
    public List<KitchenObjectSO> Objects => objects;
    
    public event Action<KitchenObjectSO> OnIngredientAddedEvent;

    protected override void Awake()
    {
        base.Awake();
        objects = new List<KitchenObjectSO>();
    }

	public bool TryAddIngredient(KitchenObjectSO kitchenObject)
    {
        bool result = validObjects.Contains(kitchenObject);
        result &= !objects.Contains(kitchenObject);

        if(result)
        {
            int index = GetObjectIndex(kitchenObject);
            AddIngredientServerRpc(index);
        }

        return result;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int objectIndex)
    {
        AddIngredientClientRpc(objectIndex);
    }

    [ClientRpc]
    private void AddIngredientClientRpc(int objectIndex)
    {
        objects.Add(validObjects[objectIndex]);
        OnIngredientAddedEvent?.Invoke(validObjects[objectIndex]);
    }

    private int GetObjectIndex(KitchenObjectSO data)
    {
        return validObjects.IndexOf(data);
    }
}
