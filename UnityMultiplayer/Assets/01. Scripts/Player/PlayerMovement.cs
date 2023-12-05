using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Reference")]
    [SerializeField] InputReaderSO inputReader = null;
    private Transform bodyTransform = null;
    private Rigidbody2D rb2d = null;

    [Header("Property")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float rotateSpeed = 270f;

    private Vector2 prevInputDir;

    public override void OnNetworkSpawn()
    {
        if(IsOwner == false)
            return;
        
        rb2d = GetComponent<Rigidbody2D>();
        bodyTransform = transform.Find("TankTreads");

        inputReader.OnMoveEvent += HandleMove;
    }

    private void Update()
    {
        if(IsOwner == false)
            return;
        
        float zDelta = -prevInputDir.x * rotateSpeed * Time.deltaTime;
        bodyTransform.Rotate(0f, 0f, zDelta);
    }

    private void FixedUpdate()
    {
        if(IsOwner == false)
            return;

        rb2d.velocity = bodyTransform.up * prevInputDir.y * moveSpeed;
    }

    public override void OnNetworkDespawn()
    {
        if(IsOwner == false)
            return;

        inputReader.OnMoveEvent -= HandleMove;
    }

    private void HandleMove(Vector2 dir)
    {
        prevInputDir = dir;
    }
}
