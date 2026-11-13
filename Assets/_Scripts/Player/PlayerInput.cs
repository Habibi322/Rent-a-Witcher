using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _controls;

    public event System.Action<Vector2> OnMove;
    public event System.Action OnJumpPressed;
    public event System.Action OnSignUsed;

    //новое
    public event System.Action OnInteractPressed;
    public event System.Action OnPausePressed;

    public event System.Action<Vector2> OnAardMove;
    public event System.Action<float> OnAardVertical;
    public event System.Action OnAardRelease;

    private Vector2 _moveInput;

    private void Awake()
    {
        _controls = new PlayerControls();

        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
        _controls.Player.Jump.started += _ => OnJumpPressed?.Invoke();
        _controls.Player.Sign.started += _ => OnSignUsed?.Invoke();

        //новое
        _controls.Player.Interact.started += _ => OnInteractPressed?.Invoke();
        _controls.Player.Pause.started += _ => OnPausePressed?.Invoke();

        _controls.Aard.MoveObject.performed += ctx => OnAardMove?.Invoke(ctx.ReadValue<Vector2>());
        _controls.Aard.MoveObject.canceled += _ => OnAardMove?.Invoke(Vector2.zero);

        _controls.Aard.Vertical.performed += ctx => OnAardVertical?.Invoke(ctx.ReadValue<float>());
        _controls.Aard.Vertical.canceled += _ => OnAardVertical?.Invoke(0f); 

        _controls.Aard.Release.started += _ => OnAardRelease?.Invoke();
    }

    private void OnEnable()
    {
        if (_controls != null) _controls.Enable();
    }

    private void OnDisable()
    {
        if (_controls != null) _controls.Disable();
    }

    private void Update()
    {
        OnMove?.Invoke(_moveInput);
    }
}