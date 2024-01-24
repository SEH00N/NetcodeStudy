using System;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] DeliveryRecipeUI recipePrefab;
    private Transform container;

    private void Awake()
    {
        container = transform.Find("Container");
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeAddEvent += HandleRecipeAdd;
        DeliveryManager.Instance.OnRecipeRemoveEvent += HandleRecipeRemove;
    }

    private void HandleRecipeAdd(RecipeSO recipe)
    {
        DeliveryRecipeUI instance = Instantiate(recipePrefab, container);
        instance.SetRecipe(recipe);
    }

    private void HandleRecipeRemove(int index)
    {
        Destroy(container.GetChild(index).gameObject);
    }
}
