using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] InputReaderSO inputReader = null;

    private Camera mainCamera = null;
	private Transform turretTransform = null;

    public override void OnNetworkSpawn()
    {
        if(IsOwner == false)
            return;

        turretTransform = transform.Find("TurretPivot");
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if(IsOwner == false)
            return;

        Vector2 lookAt = mainCamera.ScreenToWorldPoint(inputReader.MousePosition);
        float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg - 90f;
        turretTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
