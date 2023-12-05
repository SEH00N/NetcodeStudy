using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons : MonoBehaviour
{
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    
    public void Join()
    {
        NetworkManager.Singleton.StartClient();
    }
}
