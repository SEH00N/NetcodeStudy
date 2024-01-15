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

            }
            else // player empty
            {
                KitchenObject.SetKitchenObjectParent(player);
            }
        }
    }
}
