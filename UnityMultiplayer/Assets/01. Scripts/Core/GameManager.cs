using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance = null;

    private void Awake()
    {
        if(Instance != this) { DestroyImmediate(gameObject); return; }

        Instance = this;
        DontDestroyOnLoad(Instance);
    }
}
