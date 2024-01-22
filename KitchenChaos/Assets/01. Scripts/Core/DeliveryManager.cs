using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using Random = UnityEngine.Random;

public class DeliveryManager : NetworkBehaviour
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

    public event Action<Vector3, bool> OnDeliveredEvent;

    private float recipeTimer = 0f;
    private float recipeCooldown = 4f;
    
    private int recipeLimit = 4;

    public int SucceedRecipesCount { get; private set; } = 0;

    private void Awake()
    {
        waitingRecipes = new List<RecipeSO>();
    }

    private void Start()
    {
        recipeTimer = recipeCooldown;
    }

    private void Update()
    {
        if(IsServer == false)
            return;

        if(KitchenGameManager.Instance.GameState != KitchenGameManager.State.GamePlaying)
            return;

        recipeTimer -= Time.deltaTime; 
        if(recipeTimer <= 0f)
        {
            recipeTimer = recipeCooldown;

            if(waitingRecipes.Count < recipeLimit)
            {
                int reandomIndex = Random.Range(0, recipeList.Count);
                SpawnNewRecipeClientRpc(reandomIndex);
            }
        }
    }

    [ClientRpc]
    private void SpawnNewRecipeClientRpc(int randomIndex)
    {
        RecipeSO recipe = recipeList.recipes[randomIndex];

        waitingRecipes.Add(recipe);
        OnRecipeAddEvent?.Invoke(recipe);
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
                    DeliverCorrectServerRpc(i, counter.transform.position);
                    
                    return;
                }
            }
        }

        Debug.Log("Wrong Recipe");
        DeliverIncorrectServerRpc(counter.transform.position);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorrectServerRpc(Vector3 position)
    {
        DeliverIncorrectClientRpc(position);
    }

    [ClientRpc]
    private void DeliverIncorrectClientRpc(Vector3 position)
    {
        OnDeliveredEvent?.Invoke(position, false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectServerRpc(int index, Vector3 position)
    {
        DeliverCorrectClientRpc(index, position);
    }

    [ClientRpc]
    private void DeliverCorrectClientRpc(int index, Vector3 position)
    {
        RecipeSO recipe = waitingRecipes[index];
        
        SucceedRecipesCount++;
        waitingRecipes.Remove(recipe);
        
        OnRecipeRemoveEvent?.Invoke(index);
        OnDeliveredEvent?.Invoke(position, true);
    }
}
