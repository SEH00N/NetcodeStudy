using UnityEngine;
using System.Collections.Generic;
using System;

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
                    cuttingProgress = 0;

                    OnProgressChangedEvent?.Invoke(cuttingProgress, recipe.cuttingProgress, true);
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
                        KitchenObject.DestroySelf();
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
        if(IsEmpty == false && GetRecipe(KitchenObject.ObjectData, out CuttingRecipeSO recipe)) // has kitchen object and recipe
        {
            cuttingProgress++;
            OnAnyCutEvent?.Invoke(this);
            OnProgressChangedEvent?.Invoke(cuttingProgress, recipe.cuttingProgress, false);

            if(cuttingProgress >= recipe.cuttingProgress)
            {
                KitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(recipe.output, this);
            }
        }
    }

    private bool GetRecipe(KitchenObjectSO input, out CuttingRecipeSO recipe)
    {
        recipe = cuttingRecipes.Find(i => i.input == input);
        return (recipe != null);
    }
}
