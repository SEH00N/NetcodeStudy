using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Netcode;

public class CuttingCounter : BaseCounter, IProgressable
{
    [SerializeField] List<CuttingRecipeSO> cuttingRecipes;

    public static event Action<CuttingCounter> OnAnyCutEvent;    
    public event Action<float, float, bool> OnProgressChangedEvent;
    private int cuttingProgress = 0;

    new public static void ResetStaticData()
    {
        OnAnyCutEvent = null;
    }

    public override void Interact(Player player)
    {
        if(IsEmpty) // empty counter
        {
            if(player.IsEmpty == false) // player grabbed something
            {
                if(GetRecipe(player.KitchenObject.ObjectData, out CuttingRecipeSO recipe)) // can cutting
                {
                    player.KitchenObject.SetKitchenObjectParent(this); // change parent  
                    InteractLogicPlaceObjectServerRpc();                  
                }
            }
            else // player grabbed nothing
            {
                // do nothing
            }
        }
        else // have something
        {
            if(player.IsEmpty == false) // player grabbed something
            {
                if(player.KitchenObject.TryGetPlate(out PlateKitchenObject plate))
                {
                    if(plate.TryAddIngredient(KitchenObject.ObjectData))
                        KitchenObject.DestroyKitchenObject(KitchenObject);
                }
            }
            else // player empty
            {
                KitchenObject.SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if(IsEmpty == false && GetRecipeIndex(KitchenObject.ObjectData, out int recipeIndex)) // has kitchen object and recipe
        {
            CutObjectServerRpc(recipeIndex);
            CheckProgressDoneServerRpc(recipeIndex);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectServerRpc()
    {
        InteractLogicPlaceObjectClientRpc();
    }

    [ClientRpc]
    private void InteractLogicPlaceObjectClientRpc()
    {
        cuttingProgress = 0;
        OnProgressChangedEvent?.Invoke(0f, 1f, true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc(int recipeIndex)
    {
        CutObjectClientRpc(recipeIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CheckProgressDoneServerRpc(int recipeIndex)
    {
        CuttingRecipeSO recipe = cuttingRecipes[recipeIndex];
        if (cuttingProgress >= recipe.cuttingProgress)
        {
            KitchenObject.DestroyKitchenObject(KitchenObject);
            KitchenObject.SpawnKitchenObject(recipe.output, this);
        }
    }

    [ClientRpc]
    private void CutObjectClientRpc(int recipeIndex)
    {
        CuttingRecipeSO recipe = cuttingRecipes[recipeIndex];

        cuttingProgress++;
        OnAnyCutEvent?.Invoke(this);
        OnProgressChangedEvent?.Invoke(cuttingProgress, recipe.cuttingProgress, false);
    }

    private bool GetRecipe(KitchenObjectSO input, out CuttingRecipeSO recipe)
    {
        recipe = cuttingRecipes.Find(i => i.input == input);
        return (recipe != null);
    }

    private bool GetRecipeIndex(KitchenObjectSO input, out int index)
    {
        index = cuttingRecipes.FindIndex(i => i.input == input);
        return (index != -1);
    }
}
