using UnityEngine;

public partial class KitchenGameManager : MonoBehaviour
{
    public enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
}
