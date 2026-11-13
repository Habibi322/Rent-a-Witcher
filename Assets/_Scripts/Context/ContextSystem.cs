using System;
using System.Collections.Generic;
using UnityEngine;

public class ContextSystem
{
    private Dictionary<ContextKey, int> _states = new Dictionary<ContextKey, int>();

    // —обытие, на которое можно подписатьс€
    public event Action<ContextKey, int> OnContextChanged;

    public void DeclareContext(ContextKey key, int defaultValue)
    {
        if (!_states.ContainsKey(key))
            _states[key] = defaultValue;
    }

    public bool TrySetState(ContextKey key, int value)
    {
        if (!_states.ContainsKey(key))
        {
            Debug.LogError($"Context {key} не объ€влен!");
            return false;
        }

        if (_states[key] == value) return true; // «начение не изменилось

        _states[key] = value;
        OnContextChanged?.Invoke(key, value);
        return true;
    }

    public bool TryGetState(ContextKey key, out int value)
    {
        return _states.TryGetValue(key, out value);
    }

    public void SubscribeToContext(ContextKey key, Action<int> handler, bool notifyCurrent = false)
    {
        Action<ContextKey, int> wrapper = (k, v) => { if (k == key) handler(v); };
        OnContextChanged += wrapper;

        if (notifyCurrent && _states.TryGetValue(key, out int current))
            handler(current);
    }

    public void UnsubscribeFromContext(ContextKey key, Action<int> handler)
    {
        // ”прощенна€ отписка (в реальном проекте лучше хранить ссылки на делегаты)
        OnContextChanged -= (k, v) => { if (k == key) handler(v); };
    }
}