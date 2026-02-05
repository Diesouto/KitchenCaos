using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        // Si player tiene un objeto de cocina, destrúyelo
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
        }
    }
}
