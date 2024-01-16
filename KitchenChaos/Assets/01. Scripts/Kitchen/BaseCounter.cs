using System;
using System.Collections;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] protected Transform counterTopPoint;
    public Transform ParentTrm => counterTopPoint;

    private KitchenObject kitchenObject;
    public KitchenObject KitchenObject => kitchenObject;

    public bool IsEmpty => (kitchenObject == null);

    public virtual void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public virtual void ClearKitchenObject()
    {
        SetKitchenObject(null);
    }

    public virtual void Interact(Player player) {}
    public virtual void InteractAlternate(Player player) {}

    protected IEnumerator DelayCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
