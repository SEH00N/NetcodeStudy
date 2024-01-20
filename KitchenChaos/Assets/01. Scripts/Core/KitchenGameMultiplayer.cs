using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] KitchenObjectListSO kitchenObjectList;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnKitchenObject(KitchenObjectSO data, IKitchenObjectParent parent)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(data), parent.NetworkObject);
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectReference)
    {
        kitchenObjectReference.TryGet(out NetworkObject kitchenObjectObject);
        KitchenObject kitchenObject = kitchenObjectObject.GetComponent<KitchenObject>();

        ClearKitchenObjectFromParentClientRpc(kitchenObjectReference);
        Destroy(kitchenObject.gameObject);
    }

    [ClientRpc]
    private void ClearKitchenObjectFromParentClientRpc(NetworkObjectReference kitchenObjectReference)
    {
        kitchenObjectReference.TryGet(out NetworkObject kitchenObjectObject);
        KitchenObject kitchenObject = kitchenObjectObject.GetComponent<KitchenObject>();

        kitchenObject.ClearKitchenObjectFromParent();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int objectIndex, NetworkObjectReference parentReference)
    {
        KitchenObjectSO data = GetKitchenObjectSO(objectIndex);
        Transform instance = Instantiate(data.prefab);

        NetworkObject networkInstance = instance.GetComponent<NetworkObject>();
        networkInstance.Spawn(true);

        // SpawnKitchenObjectClientRpc(networkInstance, parentReference);

        KitchenObject kitchenObject = instance.GetComponent<KitchenObject>();

        if(parentReference.TryGet(out NetworkObject parentObject))
            if(parentObject.TryGetComponent<IKitchenObjectParent>(out IKitchenObjectParent parent))
                kitchenObject.SetKitchenObjectParent(parent);
    }

    private int GetKitchenObjectSOIndex(KitchenObjectSO so)
    {
        return kitchenObjectList.objects.IndexOf(so);
    }

    private KitchenObjectSO GetKitchenObjectSO(int index)
    {
        return kitchenObjectList[index];
    }
}
