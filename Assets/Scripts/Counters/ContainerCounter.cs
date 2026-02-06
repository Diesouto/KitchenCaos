using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Counter no tiene nada
            if (player.HasKitchenObject())
            {
                // Player tiene algo
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Player no tiene nada
            }
        }
        else
        {
            // Counter tiene algo
            if (player.HasKitchenObject())
            {
                // Player tiene algo
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player tiene un plato
                    // Movemos ingrediente al plato
                    plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // Se ha añadido el ingrediente al plato
                        GetKitchenObject().DestroySelf();
                    }
                } else
                {
                    // Player tiene algo que no es un plato
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Counter tiene un plato
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // Movemos ingrediente al plato
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // Player no tiene nada
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        // Si el jugador no tiene nada en las manos y la caja no tiene nada encima
        if (!player.HasKitchenObject() && !HasKitchenObject())
        {
            // El jugador agarra el objeto de la caja
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
