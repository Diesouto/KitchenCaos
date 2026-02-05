using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;


    [SerializeField] private KitchenObjectSO plate;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject() && platesSpawnedAmount > 0)
        {
            // Player is empty handed, give them a plate
            KitchenObject.SpawnKitchenObject(plate, player);
            platesSpawnedAmount--;

            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetPlatesSpawnedAmount()
    {
        return platesSpawnedAmount;
    }
}
