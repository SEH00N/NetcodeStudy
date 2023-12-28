using System;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;

    public override int Collect()
    {
        if(IsServer == false)
        {
            Show(false);
            return 0;
        }

        if(alreadyCollected)
            return 0;

        alreadyCollected = true;
        OnCollected?.Invoke(this);

        return coinValue;
    }

    public void Reset()
    {
        alreadyCollected = false;
    }
}
