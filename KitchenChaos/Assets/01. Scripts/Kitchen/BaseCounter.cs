using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : NetworkBehaviour, IKitchenObjectParent
{
    [SerializeField] protected Transform counterTopPoint;
    public Transform ParentTrm => counterTopPoint;

    public static event Action<BaseCounter, KitchenObject> OnAnyPlacedEvent;

    private KitchenObject kitchenObject;
    public KitchenObject KitchenObject => kitchenObject;

    public bool IsEmpty => (kitchenObject == null);

    public static void ResetStaticData()
    {
        OnAnyPlacedEvent = null;
    }

    public virtual void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnAnyPlacedEvent?.Invoke(this, kitchenObject);
    }

    public virtual void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public virtual void Interact(Player player) {}
    public virtual void InteractAlternate(Player player) {}

    protected IEnumerator DelayCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
