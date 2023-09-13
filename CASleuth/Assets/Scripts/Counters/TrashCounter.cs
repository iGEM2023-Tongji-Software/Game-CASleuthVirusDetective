using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;
    public event EventHandler OnPlateSpawned;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }
    }
    new public static void ResetStaticData() {
        OnAnyObjectTrashed = null;
    }
}
