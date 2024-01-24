using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryRecipeUI : MonoBehaviour
{
    [SerializeField] Image iconPrefab;
	private TMP_Text recipeNameText;
    private Transform iconContainer;

    private void Awake()
    {
        recipeNameText = transform.Find("RecipeNameText").GetComponent<TMP_Text>();
        iconContainer = transform.Find("IconContainer");
    }

    public void SetRecipe(RecipeSO recipe)
    {
        recipeNameText.text = recipe.recipeName;
        recipe.recipe.ForEach(r => {
            Image icon = Instantiate(iconPrefab, iconContainer);
            icon.sprite = r.sprite;
        });
    }
}
