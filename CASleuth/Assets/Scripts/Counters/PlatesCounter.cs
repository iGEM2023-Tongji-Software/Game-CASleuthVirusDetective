using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    private int virusType = 0;

    [SerializeField] private KitchenObjectSO redKitchenObjectSO;
    [SerializeField] private KitchenObjectSO yellowKitchenObjectSO;
    [SerializeField] private KitchenObjectSO greenKitchenObjectSO;
    [SerializeField] private KitchenObjectSO whiteKitchenObjectSO;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // 角色手上没有东西
            if (platesSpawnedAmount > 0)
            {
                // 病毒存放台上有病毒
                platesSpawnedAmount--;
                switch (virusType) {
                    case 0:
                        KitchenObject.SpawnKitchenObject(redKitchenObjectSO, player);
                        break;
                    case 1:
                        KitchenObject.SpawnKitchenObject(yellowKitchenObjectSO, player);
                        break;
                    case 2:
                        KitchenObject.SpawnKitchenObject(greenKitchenObjectSO, player);
                        break;
                    case 3:
                        KitchenObject.SpawnKitchenObject(whiteKitchenObjectSO, player);
                        break;
                }
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void SetVirusType(int type)
    {
        this.virusType = type;
    }
}
