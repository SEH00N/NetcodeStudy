using System;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class Player : NetworkBehaviour, IKitchenObjectParent
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
    [SerializeField] LayerMask collisionLayer;

    [Space(10f), Header("Multiplayer")]
    [SerializeField] List<Vector3> spawnPositions;

    public static event Action OnAnyPlayerSpawned;
    public static event Action<KitchenObject, Player> OnAnyPickSomethingEvent;

    public event Action<BaseCounter> OnSelectedCounterChanged;

    public bool IsWalking { get; private set; }

    private KitchenObject kitchenObject;
    public KitchenObject KitchenObject => kitchenObject;

    [SerializeField] Transform kitchenObjectHoldPoint;
    public Transform ParentTrm => kitchenObjectHoldPoint;

    public bool IsEmpty => (kitchenObject == null);

    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            DEFINE.LocalPlayer = this;
            transform.position = spawnPositions[(int)OwnerClientId];
        }

        OnAnyPlayerSpawned?.Invoke();

        if(IsServer)
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void Start()
    {
        if(IsOwner == false)
            return;

        inputReader.OnInteractEvent += HandleOnInteract;
        inputReader.OnInteractAlternateEvent += HandleOnInteractAlternate;
    }

    private void Update()
    {
        if (IsOwner == false)
            return;

        DetectCounter();
    }

	private void FixedUpdate()
    {
        if (IsOwner == false)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = inputReader.MoveInput;
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = Time.fixedDeltaTime * moveSpeed;

        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionLayer);

        if(canMove == false)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = (moveDir.x != 0) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity, moveDistance, collisionLayer);

            if(canMove)
                moveDir = moveDirX;
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = (moveDir.z != 0) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirZ, Quaternion.identity, moveDistance, collisionLayer);

                if(canMove)
                    moveDir = moveDirZ;
            }
        }
        
        if(canMove)
        {
            transform.position += moveDir * Time.fixedDeltaTime * moveSpeed;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.fixedDeltaTime * rotateSpeed);
        IsWalking = moveDir.sqrMagnitude != 0f;
    }

    private void HandleOnInteract()
    {
        if(KitchenGameManager.Instance.GamePlaying == false)
            return;

        selectedCounter?.Interact(this);
    }

    private void HandleOnInteractAlternate()
    {
        if (KitchenGameManager.Instance.GamePlaying == false)
            return;

        selectedCounter?.InteractAlternate(this);
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

    private void HandleClientDisconnect(ulong clientID)
    {
        if(clientID == OwnerClientId && (IsEmpty == false))
        {
            KitchenObject.DestroyKitchenObject(KitchenObject);
        }        
    }

    private void SetSelectedCounter(BaseCounter counter)
    {
        selectedCounter = counter;
        OnSelectedCounterChanged?.Invoke(selectedCounter);
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnAnyPickSomethingEvent?.Invoke(kitchenObject, this);
    }

    public void ClearKitchenObject()
    {
        // SetKitchenObject(null);
        kitchenObject = null;
    }

    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
        OnAnyPickSomethingEvent = null;
    }
}
