using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public void AddItem(ContextKey itemKey, int amount = 1)
    {
        if (ContextBootstrapper.ContextSystem.TryGetState(itemKey, out int currentAmount))
        {
            int newAmount = currentAmount + amount;
            ContextBootstrapper.ContextSystem.TrySetState(itemKey, newAmount);
            Debug.Log($"Получен предмет {itemKey}. Теперь у нас: {newAmount}");
        }
    }

    public bool HasItem(ContextKey itemKey, int requiredAmount = 1)
    {
        if (ContextBootstrapper.ContextSystem.TryGetState(itemKey, out int currentAmount))
        {
            return currentAmount >= requiredAmount;
        }
        return false;
    }

    public void RemoveItem(ContextKey itemKey, int amount = 1)
    {
        if (HasItem(itemKey, amount))
        {
            ContextBootstrapper.ContextSystem.TryGetState(itemKey, out int currentAmount);
            ContextBootstrapper.ContextSystem.TrySetState(itemKey, currentAmount - amount);
        }
    }
}