using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [Header("Параметры")]
    [SerializeField] private string _movingKey;
    [SerializeField] private string _aardTriggerKey;
    [SerializeField] private string _aardHoldKey;

    Vector2 _inputDirection = Vector2.zero;
    bool _isLeft; //initial value
    Transform _playerTransform;

    private void Start()
    {
        _playerTransform = _animator.transform;
    }

    public void UpdateInputDirection(Vector2 inputDirection)
    {
        _inputDirection = inputDirection;
        UpdateState();
    }

    public void ActivateAard()
    {
        _animator.SetTrigger(_aardTriggerKey);
        _animator.SetBool(_aardHoldKey, true);
    }

    public void DeactivateAard()
    {
        _animator.SetBool(_aardHoldKey, false);
    }

    private void UpdateState()
    {
        if (_inputDirection.sqrMagnitude > 0.001f) _animator.SetBool(_movingKey, true);
        else _animator.SetBool(_movingKey, false);
        if (_inputDirection.x < 0 && !_isLeft)
        {
            TurnHorizontal();
            _isLeft = true;
        }
        else if (_inputDirection.x > 0 && _isLeft)
        {
            TurnHorizontal();
            _isLeft = false;
        }
    }
     private void TurnHorizontal()
    {
        _playerTransform.localScale = new Vector3(-1 * _playerTransform.localScale.x, _playerTransform.localScale.y, _playerTransform.localScale.z);
    }

}
