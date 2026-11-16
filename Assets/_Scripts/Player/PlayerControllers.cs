using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private SignManager signManager;
    [SerializeField] private AardManager aardManager;
    [SerializeField] private PlayerAnimator _animatorController;

    //новое
    [SerializeField] private PlayerInteraction playerInteraction;

    private void Awake()
    {
        playerInput.OnMove += HandleMove;
        playerInput.OnJumpPressed += HandleJump;
        playerInput.OnSignUsed += HandleSign;
        playerInput.OnAardMove += HandleAardMove;
        playerInput.OnAardVertical += HandleAardVertical;
        playerInput.OnAardRelease += HandleAardRelease;

        //новое
        playerInput.OnInteractPressed += HandleInteract;
    }

    //новое
    private void HandleInteract()
    {
        // Если мы не в режиме Аарда, пробуем взаимодействовать
        if (!aardManager.IsAardActive)
        {
            playerInteraction.TryInteract();
        }
    }

    private void HandleMove(Vector2 direction)
    {
        if (!aardManager.IsAardActive)
        {
            playerMovement.SetMoveInput(direction);
            _animatorController.UpdateInputDirection(direction);
        }
    }

    private void HandleJump()
    {
        if (!aardManager.IsAardActive)
            playerMovement.TryJump();
    }

    private void HandleSign()
    {
        if (aardManager.IsAardActive)
        {
            aardManager.DeactivateAard();
            _animatorController.DeactivateAard();
        }
        else
        {
            if (signManager.TryUseSign())
            {
                _animatorController.ActivateAard();
            }
        }
    }

    private void HandleAardMove(Vector2 direction) => aardManager.UpdateAardMovement(direction);

    private void HandleAardVertical(float input) => aardManager.UpdateAardVertical(input);

    private void HandleAardRelease() 
    { 
        aardManager.DeactivateAard();
        _animatorController.DeactivateAard();
    }

    private void OnDestroy()
    {
        playerInput.OnMove -= HandleMove;
        playerInput.OnJumpPressed -= HandleJump;
        playerInput.OnSignUsed -= HandleSign;
        playerInput.OnAardMove -= HandleAardMove;
        playerInput.OnAardVertical -= HandleAardVertical;
        playerInput.OnAardRelease -= HandleAardRelease;

        //новое
        playerInput.OnInteractPressed -= HandleInteract;
    }
}