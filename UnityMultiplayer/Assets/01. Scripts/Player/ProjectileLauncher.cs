using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("Input Reader")]
    [SerializeField] InputReaderSO inputReader = null;
    
    [Header("References")]
    [SerializeField] Transform firePosition = null;
    [SerializeField] Collider2D playerCollider = null;
    [SerializeField] GameObject muzzleFlash = null;
    [SerializeField] GameObject serverProjectilePrefab = null;
    [SerializeField] GameObject clientProjectilePrefab = null;

    [Header("Property")]
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float muzzleFlashDuration = 0.5f;

    private bool shouldFire = false;
    private float lastFireTime = 0;

    public override void OnNetworkSpawn()
    {
        if(IsOwner == false)
            return;

        inputReader.OnPrimaryFireEvent += HandlePrimaryFire;
    }

    // private void Update()
    // {
    //     if(IsOwner == false)
    //         return;

    //     if(shouldFire == false)
    //         return;

    // }

    public override void OnNetworkDespawn()
    {
        if(IsOwner == false)
            return;

        inputReader.OnPrimaryFireEvent -= HandlePrimaryFire;
    }

    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        muzzleFlash.SetActive(true);
        StartCoroutine(DelayCoroutine(muzzleFlashDuration, () => muzzleFlash.SetActive(false)));

        GameObject projectile = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.up = direction;
    }

    private void HandlePrimaryFire(bool performed)
    {
        shouldFire = performed;

        if(IsOwner == false)
            return;

        if(performed == false)
            return;

        PrimaryFireServerRPC(firePosition.position, firePosition.up);
        SpawnDummyProjectile(firePosition.position, firePosition.up);
    }

    [ServerRpc]
    private void PrimaryFireServerRPC(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectile = Instantiate(serverProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.up = direction;

        Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());

        PrimaryFireClientRPC(spawnPos, direction);
    }

    [ClientRpc]
    private void PrimaryFireClientRPC(Vector3 spawnPos, Vector3 direction)
    {
        if(IsOwner)
            return;

        SpawnDummyProjectile(spawnPos, direction);
    }
    
    private IEnumerator DelayCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
