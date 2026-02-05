using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Counter no tiene nada
            if (player.HasKitchenObject())
            {
                // Player tiene algo
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else
            {
                // Player no tiene nada
            }
        } else
        {
            // Counter tiene algo
            if (player.HasKitchenObject())
            {
                // Player tiene algo
            } else
            {
                // Player no tiene nada
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
