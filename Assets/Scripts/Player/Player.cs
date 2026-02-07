using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float interactDistance = 2f;
    [SerializeField] GameInput gameInput;
    [SerializeField] LayerMask countersLayerMask;
    [SerializeField] Transform hands;

    bool isWalking = false;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction; ;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(
            transform.position, 
            transform.position + Vector3.up * playerHeight, 
            playerRadius,
            moveDir,
            moveDistance
        );

        isWalking = moveDir != Vector3.zero;

        if (!canMove)
        {
            // No se puede mover en la dirección completa
            // Intentar mover solo en X
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(
                transform.position, 
                transform.position + Vector3.up * playerHeight, 
                playerRadius,
                moveDirX,
                moveDistance
            );

            if (canMove)
            {
                // Se puede mover solo en X
                moveDir = moveDirX;
            }
            else
            {
                // No se puede mover en X tampoco
                // Intentar mover solo en Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(
                    transform.position, 
                    transform.position + Vector3.up * playerHeight, 
                    playerRadius,
                    moveDirZ,
                    moveDistance
                );

                if (canMove)
                {
                    // Se puede mover solo en Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // No se puede mover en ninguna dirección
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }

    void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Hay un ClearCounter frente al jugador
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            } else
            {
                SetSelectedCounter(null);
            }
        } else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = this.selectedCounter
        });
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return hands;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
