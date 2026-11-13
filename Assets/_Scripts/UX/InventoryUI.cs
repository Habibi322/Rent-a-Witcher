using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Настройки UI")]
    [SerializeField] private Transform slotsContainer; // Сюда будем спавнить ячейки
    [SerializeField] private GameObject slotPrefab;    // Наш префаб ячейки
    [SerializeField] private int maxSlots = 5;         // Сколько всего ячеек

    [Header("Спрайты предметов")]
    [SerializeField] private Sprite boardsSprite;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private Dictionary<ContextKey, Sprite> itemSprites = new Dictionary<ContextKey, Sprite>();

    private void Start()
    {
        // Заполняем словарь "Ключ -> Спрайт"
        itemSprites[ContextKey.Item_Boards] = boardsSprite;

        // Создаем ячейки
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            slots.Add(slot);
            slot.ClearSlot();
        }

        // Подписываемся на изменения ВСЕХ предметов
        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.SubscribeToContext(ContextKey.Item_Boards, OnItemChanged, true);
        }
    }

    private void OnDestroy()
    {
        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Item_Boards, OnItemChanged);
        }
    }

    // Вызывается, когда меняется количество любого предмета
    private void OnItemChanged(int newAmount)
    {
        // Нам нужно понять, какой именно ключ изменился. 
        // В вашей ContextSystem событие не передает ключ, поэтому мы просто обновляем весь инвентарь.
        RefreshInventory();
    }

    private void RefreshInventory()
    {
        // Очищаем все ячейки
        foreach (var slot in slots) slot.ClearSlot();

        int slotIndex = 0;

        // Проходим по всем нашим предметам и проверяем их количество
        foreach (var kvp in itemSprites)
        {
            ContextKey key = kvp.Key;
            Sprite sprite = kvp.Value;

            if (ContextBootstrapper.ContextSystem.TryGetState(key, out int amount) && amount > 0)
            {
                if (slotIndex < slots.Count)
                {
                    slots[slotIndex].SetItem(sprite, amount);
                    slotIndex++;
                }
            }
        }
    }
}