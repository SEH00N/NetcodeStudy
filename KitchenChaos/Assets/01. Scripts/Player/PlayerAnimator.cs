using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] Player player;
	private Animator animator = null;

    private readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(IsOwner == false)
            return;

        animator.SetBool(IsWalkingHash, player.IsWalking);
    }
}
