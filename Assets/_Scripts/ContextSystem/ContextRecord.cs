using UnityEngine.Events;

[System.Serializable]
public class ContextRecord
{
    public ContextKey ContextKey;
    public UnityEvent<int> EventToSubscribe = new UnityEvent<int>();
    public bool NotifyOnSubscribe;
}
