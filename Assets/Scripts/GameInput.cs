using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized()     {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        // Already normalized by Input System, but just in case
        return inputVector.normalized;
    }
}
