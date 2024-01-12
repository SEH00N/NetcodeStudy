using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private void Awake()
    {
        if(Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }
}
