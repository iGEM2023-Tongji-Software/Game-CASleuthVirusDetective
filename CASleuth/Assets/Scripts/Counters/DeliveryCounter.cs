using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }
    public event EventHandler OnPlateSpawned;

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TrytoDeliver(out KitchenObject kitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(kitchenObject);
                player.GetKitchenObject().DestroySelf();
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }

    }

    
}

