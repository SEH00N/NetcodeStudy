using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Kitchen/RecipeList")]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipes;
    public int Count => recipes.Count;
}
