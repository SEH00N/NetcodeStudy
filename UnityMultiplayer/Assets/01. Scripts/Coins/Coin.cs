using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    private SpriteRenderer spriteRenderer;

    protected int coinValue = 10;
    protected bool alreadyCollected;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public abstract int Collect();

    public void SetValue(int value) => coinValue = value;

    public void Show(bool show) => spriteRenderer.enabled = show;
}
