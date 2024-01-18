using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour
{
    private static DeliveryManager instance = null;
    public static DeliveryManager Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<DeliveryManager>();
            return instance;
        }
    }

    [SerializeField] RecipeListSO recipeList;
	private List<RecipeSO> waitingRecipes;

    public event Action<RecipeSO> OnRecipeAddEvent;
    public event Action<int> OnRecipeRemoveEvent;

    public event Action<DeliveryCounter, bool> OnDeliveredEvent;

    private float recipeTimer = 0f;
    private float recipeCooldown = 4f;
    
    private int recipeLimit = 4;

    public int SucceedRecipesCount { get; private set; } = 0;

    private void Awake()
    {
        waitingRecipes = new List<RecipeSO>();
    }

    private void Update()
    {
        recipeTimer -= Time.deltaTime; 
        if(recipeTimer <= 0f)
        {
            recipeTimer = recipeCooldown;

            if(waitingRecipes.Count < recipeLimit)
            {
                RecipeSO recipe = recipeList.recipes.PickRandom();
                waitingRecipes.Add(recipe);
                OnRecipeAddEvent?.Invoke(recipe);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plate, DeliveryCounter counter)
    {
        for(int i = 0; i < waitingRecipes.Count; i++)
        {
            RecipeSO r = waitingRecipes[i];
            if(r.recipe.Count == plate.Objects.Count)
            {
                if(r.recipe.Except(plate.Objects).ToList().Count == 0)
                {
                    Debug.Log("Recipe Found");
                    waitingRecipes.Remove(r);
                    SucceedRecipesCount++;
                    OnRecipeRemoveEvent?.Invoke(i);
                    OnDeliveredEvent?.Invoke(counter, true);
                    
                    return;
                }
            }
        }

        Debug.Log("Wrong Recipe");
        OnDeliveredEvent?.Invoke(counter, false);
    }
}