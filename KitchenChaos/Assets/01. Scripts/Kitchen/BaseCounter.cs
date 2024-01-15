using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] protected Transform counterTopPoint;
    public Transform ParentTrm => counterTopPoint;

    [SerializeField] protected KitchenObjectSO kitchenObjectData;

    protected KitchenObject kitchenObject;
    public KitchenObject KitchenObject => kitchenObject;

    public bool IsEmpty => (kitchenObject == null);

    public virtual void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public virtual void ClearKitchenObject()
    {
        SetKitchenObject(null);
    }

    public virtual void Interact(Player player) {}
}
