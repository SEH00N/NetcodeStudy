using UnityEngine;

public class PlayerAnimator : MonoBehaviour
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
        animator.SetBool(IsWalkingHash, player.IsWalking);
    }
}
