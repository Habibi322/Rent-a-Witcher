using UnityEngine;
using System.Collections.Generic;

public class ContextRegister : MonoBehaviour
{
    [SerializeField] private List<ContextRecord> _records = new List<ContextRecord>();

    void Start()
    {
        if (ContextBootstrapper.ContextSystem == null)
        {
            Debug.LogError("ContextSystem is null. Please add ContextBootstrapper to the scene");
        }
        foreach (var record in _records)
        {
            if (record?.EventToSubscribe != null)
            {
                ContextBootstrapper.ContextSystem.SubscribeToContext(
                    record.ContextKey, 
                    record.EventToSubscribe.Invoke, 
                    record.NotifyOnSubscribe);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var record in _records)
        {
            if (record?.EventToSubscribe != null)
            {
                ContextBootstrapper.ContextSystem.UnsubscribeFromContext(record.ContextKey, record.EventToSubscribe.Invoke);
            }
        }
    }
}
