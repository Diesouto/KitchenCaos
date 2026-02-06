using System.Collections.Generic;
using UnityEngine;

// Comentado para solo tener una en todo el proyecto
//[CreateAssetMenu(fileName = "RecipeListSO", menuName = "ScriptableObjects/RecipeListSO")]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipeSOList;
}
