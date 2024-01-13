using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [field : SerializeField]
    public KitchenObjectSO ObjectData { get; private set; }

    private IKitchenObjectParent kitchenObjectParent;
    public IKitchenObjectParent KitchenObjectParent => kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    {
        if(kitchenObjectParent != null)
            kitchenObjectParent.ClearKitchenObject();
        
        kitchenObjectParent = parent;

        if(kitchenObjectParent.IsEmpty == false)
            Debug.Log("!!");

        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = parent.ParentTrm;
        transform.localPosition = Vector3.zero;
    }
}
