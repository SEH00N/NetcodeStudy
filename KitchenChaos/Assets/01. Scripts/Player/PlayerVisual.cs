using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
	[SerializeField] MeshRenderer bodyMeshRenderer;
	[SerializeField] MeshRenderer headMeshRenderer;

    private Material material;

    private void Awake()
    {
        material = new Material(headMeshRenderer.material);
        headMeshRenderer.material = material;
        bodyMeshRenderer.material = material;
    }

    public void SetPlayerColorByID(int id)
    {
        Color color = KitchenGameMultiplayer.Instance.GetPlayerColor(id);
        SetPlayerColor(color);
    }

    public void SetPlayerColor(Color color)
    {
        material.color = color;
    }
}
