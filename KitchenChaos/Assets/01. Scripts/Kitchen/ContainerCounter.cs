using UnityEngine;

public class ContainerCounter : BaseCounter
{
	public override void Interact(Player player)
    {
        base.Interact(player);

        if(kitchenObject == null)
        {
            Transform instance = Instantiate(kitchenObjectData.prefab, counterTopPoint);
            instance.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else
        {
            kitchenObject.SetKitchenObjectParent(player);
        }
    }
}
