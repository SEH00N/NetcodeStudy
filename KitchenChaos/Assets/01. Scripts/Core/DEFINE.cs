using UnityEngine;

public static class DEFINE
{
    private static Player player = null;
    public static Player Player {
        get {
            if(player == null)
                player = GameObject.FindObjectOfType<Player>();
            return player;
        }
    }
}
