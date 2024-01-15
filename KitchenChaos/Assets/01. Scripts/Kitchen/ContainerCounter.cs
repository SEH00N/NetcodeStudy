using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] KitchenObjectSO kitchenObjectData;

    public event Action OnPlayerGraabedEvent;

	public override void Interact(Player player)
    {
        if(player.IsEmpty == false)
            return;

        KitchenObject.SpawnKitchenObject(kitchenObjectData, this);
        OnPlayerGraabedEvent?.Invoke();
    }
}
