using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
	[SerializeField] RespawningCoin coinPrefab;

    [SerializeField] int maxCoin = 50;
    [SerializeField] int coinValue = 10;
    [SerializeField] Vector2 xSpawnRange;
    [SerializeField] Vector2 ySpawnRange;
    [SerializeField] LayerMask blockedLayer;

    private float coinRadius = 0f;

    public override void OnNetworkSpawn()
    {
        if(IsServer == false)
            return;

        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;
        for(int i = 0; i < maxCoin; ++i)
            SpawnCoin();
    }

    private void SpawnCoin()
    {

        RespawningCoin instance = Instantiate(coinPrefab, GetSpawnPoint(), Quaternion.identity);
        instance.SetValue(coinValue);
        instance.GetComponent<NetworkObject>().Spawn();

        instance.OnCollected += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawningCoin coin)
    {
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
        if(IsServer == false)
            coin.Show(true);
    }

    private Vector2 GetSpawnPoint()
    {
        float x = 0f;
        float y = 0f;
        Vector2 point = Vector2.zero;
        
        while(true)
        {
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            point = new Vector2(x, y);

            Collider2D other = Physics2D.OverlapCircle(point, coinRadius);
            if(other == null)
                break;
        }

        return point;
    }
}
