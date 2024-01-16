using System;
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
        timer += Time.deltaTime;

        if(timer >= spawnTime)
        {
            timer = 0f;

            if(spawnLimit > spawnedAmount)
            {
                spawnedAmount++;
                OnPlateSpawnedEvent?.Invoke();
            }
        }
    }

    public override void Interact(Player player)
    {
        if(player.IsEmpty) // player is empty
        {
            if(spawnedAmount > 0) // has plate
            {
                spawnedAmount--;
                OnPlateRemovedEvent?.Invoke();

                KitchenObject.SpawnKitchenObject(plateData, player);
            }
        }
    }
}
