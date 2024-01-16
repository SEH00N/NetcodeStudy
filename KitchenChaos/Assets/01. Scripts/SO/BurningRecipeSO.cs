using UnityEngine;

[CreateAssetMenu(menuName = "SO/Kitchen/BurningRecipe")]
public class BurningRecipeSO : ScriptableObject
{
	public KitchenObjectSO input;
	public KitchenObjectSO output;
    public float burningTime = 1f;
}
