using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    private static ClientSingleton instance = null;
    public static ClientSingleton Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<ClientSingleton>();
            return instance;
        }
    }

    public ClientGameManager GameManager { get; private set; } = null;

	private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public async Task<bool> CreateClient()
    {
        GameManager = new ClientGameManager();
        return await GameManager.InitAsync();
    }
}
