using Unity.Netcode;
using UnityEngine;

public interface IKitchenObjectParent
{
    public KitchenObject KitchenObject { get; }
    public Transform ParentTrm { get; }
    public bool IsEmpty { get; }
    public NetworkObject NetworkObject { get; }

    public void SetKitchenObject(KitchenObject kitchenObject);
    public void ClearKitchenObject();
}
