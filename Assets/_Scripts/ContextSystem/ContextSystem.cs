using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContextSystem
{
    Dictionary<ContextKey, ContextState> _contextEvents = new Dictionary<ContextKey, ContextState>();


    public bool DeclareContext(ContextKey key, int initialValue)
    {
        if (_contextEvents.TryGetValue(key, out var state))
        {
            Debug.LogWarning($"Attmept to repeat decraling of context {key}");
            return false;
        }
        state = new ContextState();
        state.ChangeStateWithoutNotify(initialValue);
        _contextEvents.Add(key, state);
        return true;
    }

    public bool SubscribeToContext(ContextKey key, UnityAction<int> subscriber, bool notifyCurrent)
    {
        if (!_contextEvents.TryGetValue(key, out ContextState state))
        {
            Debug.LogWarning($"Attmept to subscribe to context {key} before declare ");
            return false;
        }

        state.OnChangedState.AddListener(subscriber);
        if (notifyCurrent) subscriber.Invoke(state.GetState());

        return true;
    }

    public bool UnsubscribeFromContext(ContextKey key, UnityAction<int> subscriber)
    {
        if (!_contextEvents.TryGetValue(key, out ContextState state))
        {
            Debug.LogWarning($"Attmept to unsubscribe from context {key} that was not declare");
            return false;
        }
        
        state.OnChangedState.RemoveListener(subscriber);
        return true;
    }

    public bool TryGetState(ContextKey key, out int currentState)
    {
        currentState = default;
        if (!_contextEvents.TryGetValue(key, out ContextState state)) return false;
        currentState = state.GetState();
        return true;
    }

    

    public bool TrySetState(ContextKey key, int newState)
    {
        if (!_contextEvents.TryGetValue(key, out ContextState state)) return false;
        state.ChangeState(newState);
        return true;
    }

    public bool TrySetStateWithoutNotify(ContextKey key, int newState)
    {
        if (!_contextEvents.TryGetValue(key, out ContextState state)) return false;
        state.ChangeStateWithoutNotify(newState);
        return true;
    }

}
