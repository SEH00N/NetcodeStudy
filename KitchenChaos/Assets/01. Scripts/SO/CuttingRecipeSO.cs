using UnityEngine;

[CreateAssetMenu(menuName = "SO/Kitchen/CuttingRecipe")]
public class CuttingRecipeSO : ScriptableObject
{
	public KitchenObjectSO input;
	public KitchenObjectSO output;
    public int cuttingProgress = 3;
}
