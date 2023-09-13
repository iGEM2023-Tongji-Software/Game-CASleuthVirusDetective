using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // 柜子上没有物品
            if (player.HasKitchenObject())
            {
                // 角色有物品，放置物品
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // 角色没有物品
            }
        }
        else
        {
            // 柜子上有物品
            if (player.HasKitchenObject())
            {
                // 角色有物品
            }
            else
            {
                // 角色没有物品，拾取物品
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
