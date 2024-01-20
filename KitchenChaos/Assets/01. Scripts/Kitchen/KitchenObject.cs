using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [field : SerializeField]
    public KitchenObjectSO ObjectData { get; private set; }

    private IKitchenObjectParent kitchenObjectParent;
    public IKitchenObjectParent KitchenObjectParent => kitchenObjectParent;

    private FollowTransform followTransform;

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }

    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    {
        SetKitchenObjectParentServerRpc(parent.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference parentReference)
    {
        SetKitchenObjectParentClientRpc(parentReference);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference parentReference)
    {
        if(kitchenObjectParent != null)
        {
            Debug.Log($"last kitchen object parent is existing : {kitchenObjectParent.ParentTrm.root.name}");
            kitchenObjectParent.ClearKitchenObject();
        }

        parentReference.TryGet(out NetworkObject parentObject);
        IKitchenObjectParent parent = parentObject?.GetComponent<IKitchenObjectParent>();
        
        kitchenObjectParent = parent;
        if(parent.IsEmpty == false)
            Debug.LogError("!!");

        kitchenObjectParent.SetKitchenObject(this);

        followTransform.SetTargetTransform(parent.ParentTrm);
    }

    public void ClearKitchenObjectFromParent()
    {
        kitchenObjectParent.ClearKitchenObject();
    }

    public bool TryGetPlate(out PlateKitchenObject plate)
    {
        plate = this as PlateKitchenObject;
        return plate != null;
    }

    public static void SpawnKitchenObject(KitchenObjectSO data, IKitchenObjectParent parent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(data, parent);
    }

    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    }
}
