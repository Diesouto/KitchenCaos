using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Counter no tiene nada
            if (player.HasKitchenObject())
            {
                // Player tiene algo
                //if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                //    // El objeto que el jugador tiene se puede cortar
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
        if (HasKitchenObject())
        {
            KitchenObjectSO cutItem = GetOutputFromInput(GetKitchenObject().GetKitchenObjectSO());

            if (cutItem) {
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(cutItem, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }
}

