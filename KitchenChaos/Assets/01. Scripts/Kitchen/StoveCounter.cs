using System;
using System.Collections.Generic;
using UnityEngine;

public partial class StoveCounter : BaseCounter, IProgressable
{
    [SerializeField] List<FryingRecipeSO> fryingRecipes;
    [SerializeField] List<BurningRecipeSO> burningRecipes;

    public event Action<State> OnStateChangedEvent;
    public event Action<float, float, bool> OnProgressChangedEvent;

    private float timer = 0f;
    private FryingRecipeSO fryingRecipe;
    private BurningRecipeSO burningRecipe;
    private State state;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (IsEmpty)
            return;

        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                timer += Time.deltaTime;

                if (timer >= fryingRecipe.fryingTime)
                {
                    KitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipe.output, this);

                    GetRecipe(KitchenObject.ObjectData, out burningRecipe);
                    ChangeState(State.Fried);
                }
                else
                    OnProgressChangedEvent?.Invoke(timer, fryingRecipe.fryingTime, false);

                break;
            case State.Fried:
                timer += Time.deltaTime;

                if (timer >= burningRecipe.burningTime)
                {
                    KitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipe.output, this);

                    ChangeState(State.Burned);
                }
                else
                    OnProgressChangedEvent?.Invoke(timer, burningRecipe.burningTime, false);

                break;
            case State.Burned:
                break;
        }
    }

    public override void Interact(Player player)
    {
        if(IsEmpty) // empty counter
        {
            if(player.IsEmpty == false) // player grabbed something
            {
                if(GetRecipe(player.KitchenObject.ObjectData, out fryingRecipe)) // can cutting
                {
                    player.KitchenObject.SetKitchenObjectParent(this); // change parent
                    ChangeState(State.Frying);
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
                    {
                        KitchenObject.DestroySelf();
                        ChangeState(State.Idle);
                    }
                }
            }
            else // player empty
            {
                KitchenObject.SetKitchenObjectParent(player);
                ChangeState(State.Idle);
            }
        }
    }

    public void ChangeState(State state)
    {
        timer = 0f;
        OnStateChangedEvent?.Invoke(state);
        OnProgressChangedEvent?.Invoke(0f, 1f, true);

        this.state = state;

    }

    private bool GetRecipe(KitchenObjectSO input, out FryingRecipeSO recipe)
    {
        recipe = fryingRecipes.Find(i => i.input == input);
        return (recipe != null);
    }

    private bool GetRecipe(KitchenObjectSO input, out BurningRecipeSO recipe)
    {
        recipe = burningRecipes.Find(i => i.input == input);
        return (recipe != null);
    }
}
