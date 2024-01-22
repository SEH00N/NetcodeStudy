using Unity.Netcode;
using UnityEngine;

public partial class KitchenGameManager : NetworkBehaviour
{
    public enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
}
