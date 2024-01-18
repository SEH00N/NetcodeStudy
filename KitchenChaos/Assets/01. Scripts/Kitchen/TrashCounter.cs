using System;
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
            OnAnyTrashedEvent?.Invoke(this);
            player.KitchenObject.DestroySelf();
        }
    }
}
