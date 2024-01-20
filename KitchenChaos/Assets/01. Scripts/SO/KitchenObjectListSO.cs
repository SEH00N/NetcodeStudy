using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Kitchen/KitchenObjectList")]
public class KitchenObjectListSO : ScriptableObject
{
    public List<KitchenObjectSO> objects;
    public int Count => objects.Count;

    public KitchenObjectSO this[int index] => objects[index];
}
