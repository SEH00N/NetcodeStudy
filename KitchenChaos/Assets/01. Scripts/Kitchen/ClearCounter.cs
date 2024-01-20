using UnityEngine;

public class ClearCounter : BaseCounter
{
	public override void Interact(Player player)
    {
        if(IsEmpty) // empty counter
        {
            if(player.IsEmpty == false) // player grabbed something
            {
                player.KitchenObject.SetKitchenObjectParent(this); // change parent
            }
            else // player grabbed nothing
            {
                // do nothing
            }
        }
        else // have something
        {
            if(player.IsEmpty == false) // player grabbed something
            {
                PlateKitchenObject plate = null;
                if(player.KitchenObject.TryGetPlate(out plate))
                {
                    if(plate.TryAddIngredient(KitchenObject.ObjectData))
                        KitchenObject.DestroyKitchenObject(KitchenObject);
                }
                else // player grabbed food
                {
                    if(KitchenObject.TryGetPlate(out plate))
                    {
                        if(plate.TryAddIngredient(player.KitchenObject.ObjectData))
                            KitchenObject.DestroyKitchenObject(player.KitchenObject);
                    }
                }
            }
            else // player empty
            {
                KitchenObject.SetKitchenObjectParent(player);
            }
        }
    }
}
