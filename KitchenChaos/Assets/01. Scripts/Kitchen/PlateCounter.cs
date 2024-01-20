using System;
using Unity.Netcode;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    [SerializeField] KitchenObjectSO plateData;
    [SerializeField] float spawnTime = 4f;
    [SerializeField] int spawnLimit = 4;

    // <amount>
    public event Action OnPlateSpawnedEvent;
    public event Action OnPlateRemovedEvent;

	private float timer = 0f;
    private int spawnedAmount = 0;

    private void Update()
    {
        if(IsServer == false)
            return;

        timer += Time.deltaTime;

        if(timer >= spawnTime)
        {
            timer = 0f;

            if(spawnLimit > spawnedAmount)
            {
                SpawnPlateServerRpc();
            }
        }
    }

    public override void Interact(Player player)
    {
        if(player.IsEmpty) // player is empty
        {
            if(spawnedAmount > 0) // has plate
            {
                KitchenObject.SpawnKitchenObject(plateData, player);
                InteractLogicServerRpc();
            }
        }
    }

    [ServerRpc]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }

    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        spawnedAmount++;
        OnPlateSpawnedEvent?.Invoke();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        spawnedAmount--;
        OnPlateRemovedEvent?.Invoke();
    }
}
