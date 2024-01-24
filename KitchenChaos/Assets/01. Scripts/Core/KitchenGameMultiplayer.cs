using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }
    private const uint MAX_PLAYER_AMOUNT = 4;

    [SerializeField] KitchenObjectListSO kitchenObjectList;
    [SerializeField] List<Color> playerColors;

    public event Action OnTryingToJoinGameEvent;
    public event Action OnFailedToJoinGameEvent;
    public event Action<NetworkListEvent<PlayerData>> OnPlayerDataNetworkListChangedEvent;

    private NetworkList<PlayerData> playerDataNetworkList;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += HandleListChanged;
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += HandleConnectionApproval;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect_Server;
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        OnTryingToJoinGameEvent?.Invoke();
        
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect_Client;
        NetworkManager.Singleton.StartClient();
    }

    private void HandleListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChangedEvent?.Invoke(changeEvent);
    }

    private void HandleClientConnected(ulong clientID)
    {
        playerDataNetworkList.Add(new PlayerData() {
            clientID = clientID,
            colorID = GetFirstUnusedColorID()
        });
    }


    private void HandleClientDisconnect_Server(ulong clientID)
    {
        for(int i = 0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = playerDataNetworkList[i];
            if(playerData.clientID == clientID)
            {
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void HandleClientDisconnect_Client(ulong clientID)
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

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }

    public PlayerData GetLocalPlayerData()
    {
        GetPlayerDataByID(NetworkManager.Singleton.LocalClientId, out PlayerData playerData);
        return playerData;
    }

    public PlayerData GetPlayerDataByIndex(int playerIndex)
    {
        return playerDataNetworkList[playerIndex];
    }

    public bool GetPlayerDataByID(ulong playerID, out PlayerData playerData)
    {
        foreach(PlayerData data in playerDataNetworkList)
        {
            if(data.clientID == playerID)
            {
                playerData = data;
                return true;
            }
        }
        playerData = default;
        return false;
    }

    public bool GetPlayerIndexByID(ulong playerID, out int playerIndex)
    {
        for(int i = 0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData data = playerDataNetworkList[i];
            if(data.clientID == playerID)
            {
                playerIndex = i;
                return true;
            }
        }
        playerIndex = -1;
        return false;
    }

    public Color GetPlayerColor(int colorID)
    {
        return playerColors[colorID];
    }

    public void ChangePlayerColor(int colorID)
    {
        ChangePlayerColorServerRpc(colorID);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerColorServerRpc(int colorID, ServerRpcParams serverRpcParams = default)
    {
        if(IsColorAvailable(colorID) == false)
            return;

        if(GetPlayerIndexByID(serverRpcParams.Receive.SenderClientId, out int playerID))
        {
            PlayerData playerData = playerDataNetworkList[playerID];
            playerData.colorID = colorID;
            playerDataNetworkList[playerID] = playerData;
        }
    }

    private bool IsColorAvailable(int colorID)
    {
        foreach(PlayerData playerData in playerDataNetworkList)
            if(playerData.colorID == colorID)
                return false;

        return true;
    }

    private int GetFirstUnusedColorID()
    {
        for(int i = 0; i < playerColors.Count; i++)
            if(IsColorAvailable(i))
                return i;

        return -1;
    }

    public void KickPlayer(ulong clientID)
    {
        NetworkManager.Singleton.DisconnectClient(clientID);
        HandleClientDisconnect_Server(clientID);
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
