using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Kitchen/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<KitchenObjectSO> recipe;
}
