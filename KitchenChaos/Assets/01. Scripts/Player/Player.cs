using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] InputReaderSO inputReader = null;

    [Space(10f), Header("Property")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotateSpeed = 10f;

    [Space(10f), Header("Player Setting")]
    [SerializeField] float playerHeight = 2f;
    [SerializeField] float playerRadius = 0.7f;

    [Space(10f), Header("Interact")]
    [SerializeField] float interactDistance = 2f;
    [SerializeField] LayerMask counterLayer;

    public event Action<BaseCounter> OnSelectedCounterChanged;

    public bool IsWalking { get; private set; }

    private KitchenObject kitchenObject;
    public KitchenObject KitchenObject => kitchenObject;

    [SerializeField] Transform kitchenObjectHoldPoint;
    public Transform ParentTrm => kitchenObjectHoldPoint;

    public bool IsEmpty => (kitchenObject == null);

    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;

    private void Start()
    {
        inputReader.OnInteractEvent += HandleOnInteract;
    }

    private void Update()
    {
        DetectCounter();
    }

	private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = inputReader.MoveInput;
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = Time.fixedDeltaTime * moveSpeed;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if(canMove == false)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if(canMove)
                moveDir = moveDirX;
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if(canMove)
                    moveDir = moveDirZ;
            }
        }
        
        if(canMove)
        {
            transform.position += moveDir * Time.fixedDeltaTime * moveSpeed;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.fixedDeltaTime * rotateSpeed);
        }

        IsWalking = moveDir.sqrMagnitude > 0f;
    }

    private void HandleOnInteract()
    {
        selectedCounter?.Interact(this);
    }

    private void DetectCounter()
    {
        Vector2 inputVector = inputReader.MoveInput;
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir.sqrMagnitude > 0f)
            lastInteractDirection = moveDir;

        if(Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit hit, interactDistance, counterLayer))
        {
            if (hit.transform.TryGetComponent<BaseCounter>(out BaseCounter counter))
            {
                if (selectedCounter != counter)
                    SetSelectedCounter(counter);
            }
            else
                SetSelectedCounter(null);
        }
        else
            SetSelectedCounter(null);
    }

    private void SetSelectedCounter(BaseCounter counter)
    {
        selectedCounter = counter;
        OnSelectedCounterChanged?.Invoke(selectedCounter);
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public void ClearKitchenObject()
    {
        SetKitchenObject(null);
    }
}
