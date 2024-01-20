using System;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event Action<TrashCounter> OnAnyTrashedEvent;

    new public static void ResetStaticData()
    {
        OnAnyTrashedEvent = null;
    }

    public override void Interact(Player player)
    {
        if(player.IsEmpty == false)
        {
            KitchenObject.DestroyKitchenObject(player.KitchenObject);
            InteractLogicServerRpc();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnAnyTrashedEvent?.Invoke(this);
    }
}
