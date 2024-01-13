using UnityEngine;

[CreateAssetMenu(menuName = "SO/Kitchen/KitchenObject")]
public class KitchenObjectSO : ScriptableObject
{
    public string objectName;
	public Transform prefab;
	public Sprite sprite;
}
