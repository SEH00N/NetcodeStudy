using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }
    private const uint MAX_PLAYER_AMOUNT = 4;

    [SerializeField] KitchenObjectListSO kitchenObjectList;

    public event Action OnTryingToJoinGameEvent;
    public event Action OnFailedToJoinGameEvent;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += HandleConnectionApproval;
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        OnTryingToJoinGameEvent?.Invoke();
        
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        NetworkManager.Singleton.StartClient();
    }

    private void HandleClientDisconnect(ulong clientID)
    {
        OnFailedToJoinGameEvent?.Invoke();
    }

    private void HandleConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if(SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelectScene.ToString())
        {
            response.Approved = false;
            response.Reason = "Game has already started";
            return;
        }

        if(NetworkManager.Singleton.ConnectedClients.Count >= MAX_PLAYER_AMOUNT)
        {
            response.Approved = false;
            response.Reason = "Game is full";
            return;
        }
        response.Approved = true;
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
