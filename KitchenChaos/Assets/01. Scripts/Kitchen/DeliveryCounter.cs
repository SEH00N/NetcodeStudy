using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if(player.IsEmpty == false)
        {
            if(player.KitchenObject.TryGetPlate(out PlateKitchenObject plate))
            {
                DeliveryManager.Instance.DeliverRecipe(plate, this);
                player.KitchenObject.DestroySelf();
            }
        }
    }
}
