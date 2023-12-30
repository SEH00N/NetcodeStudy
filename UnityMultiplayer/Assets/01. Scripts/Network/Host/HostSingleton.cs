using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance = null;
    public static HostSingleton Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<HostSingleton>();
            return instance;
        }
    }

    public HostGameManager GameManager { get; private set; } = null;

	private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        GameManager = new HostGameManager();
    }
}
