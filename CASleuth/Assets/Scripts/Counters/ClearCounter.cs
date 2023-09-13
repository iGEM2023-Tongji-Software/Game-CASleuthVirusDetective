using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // ������û����Ʒ
            if (player.HasKitchenObject())
            {
                // ��ɫ����Ʒ��������Ʒ
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // ��ɫû����Ʒ
            }
        }
        else
        {
            // ����������Ʒ
            if (player.HasKitchenObject())
            {
                // ��ɫ����Ʒ
            }
            else
            {
                // ��ɫû����Ʒ��ʰȡ��Ʒ
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
