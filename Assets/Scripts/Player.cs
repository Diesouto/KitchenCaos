using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] GameInput gameInput;

    bool isWalking = false;

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
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

    public bool IsWalking()
    {
        return isWalking;
    }
}
