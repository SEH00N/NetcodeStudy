using UnityEngine;

[CreateAssetMenu(menuName = "SO/Kitchen/FryingRecipe")]
public class FryingRecipeSO : ScriptableObject
{
	public KitchenObjectSO input;
	public KitchenObjectSO output;
    public float fryingTime = 1f;
}
