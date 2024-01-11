using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] InputReaderSO inputReader = null;

    [Space(10f)]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotateSpeed = 10f;

    public bool IsWalking { get; private set; }

	private void FixedUpdate()
    {
        Debug.Log(inputReader.MoveInput);
        Vector2 inputVector = inputReader.MoveInput;
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        IsWalking = moveDir.sqrMagnitude > 0f;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.fixedDeltaTime * rotateSpeed);
        transform.position += moveDir * Time.fixedDeltaTime * moveSpeed;
    }
}
