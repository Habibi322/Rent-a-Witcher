using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Context Defaults", fileName = "ContextDefaults")]
public class ContextDefaults : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public ContextKey Key;
        public int InitialValue;
    }

    [SerializeField] private List<Entry> _entries = new List<Entry>();

    public IReadOnlyList<Entry> Entries => _entries;

    public void DeclareAll(ContextSystem system)
    {
        foreach (var entry in _entries)
        {
            system.DeclareContext(entry.Key, entry.InitialValue);
        }
    }

    public void ResetAll(ContextSystem system)
    {
        foreach (var entry in _entries)
        {
            system.TrySetState(entry.Key, entry.InitialValue);
        }
    }

    private void OnValidate()
    {
        var seen = new HashSet<ContextKey>();
        foreach (var entry in _entries)
        {
            if (!seen.Add(entry.Key))
            {
                Debug.LogWarning(
                    $"ContextDefaults '{name}': ключ '{entry.Key}' объявлен дважды.",
                    this);
            }
        }
    }
}