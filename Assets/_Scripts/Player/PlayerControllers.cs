using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private SignManager signManager;
    [SerializeField] private AardManager aardManager;

    private void Awake()
    {
        playerInput.OnMove += HandleMove;
        playerInput.OnJumpPressed += HandleJump;
        playerInput.OnSignUsed += HandleSign;
        playerInput.OnAardMove += HandleAardMove;
        playerInput.OnAardVertical += HandleAardVertical;
        playerInput.OnAardRelease += HandleAardRelease;
    }

    private void HandleMove(Vector2 direction)
    {
        if (!aardManager.IsAardActive)
            playerMovement.SetMoveInput(direction);
    }

    private void HandleJump()
    {
        if (!aardManager.IsAardActive)
            playerMovement.TryJump();
    }

    private void HandleSign()
    {
        if (aardManager.IsAardActive)
            aardManager.DeactivateAard();
        else
            signManager.TryUseSign();
    }

    private void HandleAardMove(Vector2 direction) => aardManager.UpdateAardMovement(direction);

    private void HandleAardVertical(float input) => aardManager.UpdateAardVertical(input);

    private void HandleAardRelease() => aardManager.DeactivateAard();

    private void OnDestroy()
    {
        playerInput.OnMove -= HandleMove;
        playerInput.OnJumpPressed -= HandleJump;
        playerInput.OnSignUsed -= HandleSign;
        playerInput.OnAardMove -= HandleAardMove;
        playerInput.OnAardVertical -= HandleAardVertical;
        playerInput.OnAardRelease -= HandleAardRelease;
    }
}