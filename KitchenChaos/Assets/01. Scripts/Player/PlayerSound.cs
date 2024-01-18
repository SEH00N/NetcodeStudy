using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioAssetsSO audioAssets;

    [Space(10f)]
    [SerializeField] float footstepCooldown = 0.5f;
    private float footstepTimer = 0f;

	private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if(footstepTimer <= 0f)
        {
            if(player.IsWalking)
            {
                footstepTimer = footstepCooldown;
                AudioManager.Instance.PlayAudio(audioAssets.footstep, transform.position, 1f);
            }
        }
    }
}
