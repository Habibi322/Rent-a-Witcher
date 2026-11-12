using UnityEngine.Events;

public class ContextState
{
    public readonly UnityEvent<int> OnChangedState = new UnityEvent<int>();
    private int _state;

    public void ChangeStateWithoutNotify(int newState)
    {
        _state = newState;
    }

    public void ChangeState(int newState)
    {
        if (_state == newState) return;
        _state = newState;
        OnChangedState.Invoke(_state);
    }

    public int GetState() => _state;
}
