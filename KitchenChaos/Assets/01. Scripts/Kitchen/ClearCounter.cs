using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] Transform counterTopPoint;
    public Transform ParentTrm => counterTopPoint;

    [SerializeField] KitchenObjectSO kitchenObjectData;

    private KitchenObject kitchenObject;
    public KitchenObject KitchenObject => kitchenObject;

    public bool IsEmpty => (kitchenObject == null);

	public void Interact(Player player)
    {
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

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public void ClearKitchenObject()
    {
        SetKitchenObject(null);
        
    }
}
