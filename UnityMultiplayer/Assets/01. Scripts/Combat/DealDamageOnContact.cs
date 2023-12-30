using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] int damage = 5;

    private ulong ownerClientID = 0;

    public void SetOwner(ulong id)
    {
        ownerClientID = id;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == null)
            return;

        if(other.TryGetComponent<NetworkObject>(out NetworkObject networkObj) == false)
            return;

        Debug.Log(other.name);
        if(networkObj.OwnerClientId == ownerClientID)
            return;
        if (other.TryGetComponent<Health>(out Health health))
            health?.TakeDamage(damage);
    }
}
