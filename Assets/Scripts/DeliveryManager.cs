using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] RecipeListSO recipeListSO;
    [SerializeField] int waitingRecipesMax = 4;

    List<RecipeSO> waitingRecipeSOList;
    float spawnRecipeTimer;
    float spawnRecipeTimerMax = 4f;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                Debug.Log(waitingRecipeSO.recipeName);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            // Para cada receta en la lista de recetas esperando ser entregadas

            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count != plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Si la cantidad de ingredientes no coincide, no es la receta correcta
                continue;
            }

            bool plateContentsMatchesRecipe = true;

            foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
            {
                // Para cada ingrediente de la receta
                bool ingredientFound = false;

                foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                {
                    // Para cada ingrediente del plato
                    if (recipeKitchenObjectSO == plateKitchenObjectSO)
                    {
                        // El ingrediente coincide, continuar con el siguiente ingrediente de la receta
                        ingredientFound = true;
                        break;
                    }
                }

                if (!ingredientFound)
                {
                    // Si no se encuentra el ingrediente en el plato, no es la receta correcta
                    plateContentsMatchesRecipe = false;
                }
            }

            if (plateContentsMatchesRecipe)
            {
                // La receta coincide, entregar el plato
                Debug.Log("Entrega correcta");
                waitingRecipeSOList.RemoveAt(i);
                return;
            }
        }

        // No se encontró la receta
        // El jugador entregó un plato incorrecto
        Debug.Log("Entrega incorrecta");
    }
}
