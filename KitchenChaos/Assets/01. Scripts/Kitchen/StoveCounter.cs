using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public partial class StoveCounter : BaseCounter, IProgressable
{
    [SerializeField] List<FryingRecipeSO> fryingRecipes;
    [SerializeField] List<BurningRecipeSO> burningRecipes;

    public event Action<State> OnStateChangedEvent;
    public event Action<float, float, bool> OnProgressChangedEvent;

    private NetworkVariable<State> state = new NetworkVariable<State>();
    private NetworkVariable<float> timer = new NetworkVariable<float>(0f);
    private FryingRecipeSO fryingRecipe;
    private BurningRecipeSO burningRecipe;

    public override void OnNetworkSpawn()
    {
        timer.OnValueChanged += HandleTimerValueChanged;
        state.OnValueChanged += HandleStateValueChanged;
    }

    private void Start()
    {
        if (IsServer == false)
            return;

        state.Value = State.Idle;
    }

    private void Update()
    {
        if(IsServer == false)
            return;

        if (IsEmpty)
            return;

        switch (state.Value)
        {
            case State.Idle:
                break;
            case State.Frying:
                timer.Value += Time.deltaTime;

                if (timer.Value >= fryingRecipe.fryingTime)
                {
                    KitchenObject.DestroyKitchenObject(KitchenObject);
                    KitchenObject.SpawnKitchenObject(fryingRecipe.output, this);

                    GetBurningRecipeIndex(KitchenObject.ObjectData, out int recipeIndex);
                    SetBurningRecipeClientRpc(recipeIndex);
                    ChangeState(State.Fried);
                }
                break;
            case State.Fried:
                timer.Value += Time.deltaTime;

                if (timer.Value >= burningRecipe.burningTime)
                {
                    KitchenObject.DestroyKitchenObject(KitchenObject);
                    KitchenObject.SpawnKitchenObject(burningRecipe.output, this);

                    ChangeState(State.Burned);
                }

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
                if(GetFryingRecipeIndex(player.KitchenObject.ObjectData, out int recipeIndex)) // can cutting
                {
                    player.KitchenObject.SetKitchenObjectParent(this); // change parent
                    SetFryingRecipeServerRpc(recipeIndex);
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
                        KitchenObject.DestroyKitchenObject(KitchenObject);
                        SetStateServerRpc(State.Idle);
                    }
                }
            }
            else // player empty
            {
                KitchenObject.SetKitchenObjectParent(player);
                SetStateServerRpc(State.Idle);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateServerRpc(State state)
    {
        ChangeState(state);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetFryingRecipeServerRpc(int recipeIndex)
    {
        SetFryingRecipeClientRpc(recipeIndex);
        ChangeState(State.Frying);
    }

    [ClientRpc]
    private void SetFryingRecipeClientRpc(int recipeIndex)
    {
        fryingRecipe = fryingRecipes[recipeIndex];
    }

    [ClientRpc]
    private void SetBurningRecipeClientRpc(int recipeIndex)
    {
        burningRecipe = burningRecipes[recipeIndex];
    }

    public void ChangeState(State state)
    {
        timer.Value = 0f;
        this.state.Value = state;
    }

    private void HandleTimerValueChanged(float previousValue, float newValue)
    {
        float divide = 1f;
        if(state.Value == State.Frying)
            divide = fryingRecipe != null ? fryingRecipe.fryingTime : 1f;
        else if (state.Value == State.Fried)
            divide = burningRecipe != null ? burningRecipe.burningTime : 1f;

        OnProgressChangedEvent?.Invoke(newValue, divide, false);
    }

    private void HandleStateValueChanged(State previousValue, State newValue)
    {
        OnProgressChangedEvent?.Invoke(0f, 1f, true);
        OnStateChangedEvent?.Invoke(newValue);
    }

    private bool GetFryingRecipeIndex(KitchenObjectSO input, out int recipeIndex)
    {
        recipeIndex = fryingRecipes.FindIndex(i => i.input == input);
        return (recipeIndex != -1);
    }

    private bool GetBurningRecipeIndex(KitchenObjectSO input, out int recipeIndex)
    {
        recipeIndex = burningRecipes.FindIndex(i => i.input == input);
        return (recipeIndex != -1);
    }
}
