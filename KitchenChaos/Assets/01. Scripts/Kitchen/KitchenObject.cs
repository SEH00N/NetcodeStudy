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
        if(parent.IsEmpty == false)
            Debug.LogError("!!");

        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = parent.ParentTrm;
        transform.localPosition = Vector3.zero;
    }

    public void DestrySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO data, IKitchenObjectParent parent)
    {
        Transform instance = Instantiate(data.prefab);
        KitchenObject kitchenObject = instance.GetComponent<KitchenObject>();
        
        kitchenObject.SetKitchenObjectParent(parent);
        return kitchenObject;
    }
}
