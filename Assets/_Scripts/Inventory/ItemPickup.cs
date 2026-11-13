using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ContextKey itemKey;
    [SerializeField] private int amount = 1;

    public void Interact()
    {
        InventorySystem inventory = FindFirstObjectByType<InventorySystem>();

        if (inventory != null)
        {
            inventory.AddItem(itemKey, amount);
            Destroy(gameObject); // Предмет исчезает
        }
    }
}