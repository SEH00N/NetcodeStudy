using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance = null;
    public HostSingleton Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<HostSingleton>();
            return instance;
        }
    }

    private HostGameManager gameManager = null;

	private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        gameManager = new HostGameManager();
    }
}
