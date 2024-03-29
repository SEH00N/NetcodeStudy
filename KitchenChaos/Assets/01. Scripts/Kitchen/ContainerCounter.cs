using System;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] KitchenObjectSO kitchenObjectData;

    public event Action OnPlayerGraabedEvent;

	public override void Interact(Player player)
    {
        if(player.IsEmpty == false)
            return;

        KitchenObject.SpawnKitchenObject(kitchenObjectData, player);
        InteractLogicServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnPlayerGraabedEvent?.Invoke();
    }
}
