using UnityEngine;

public class FenceRepairSpot : MonoBehaviour
{
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private int boardCount = 3; // Лучше вынести в инспектор, чтобы менять без кода

    public void Interact()
    {
        // 1. Проверяем стадию квеста
        int currentQuestStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Quest_ZombieFenceStarted, out int state))
        {
            currentQuestStage = state;
        }

        // Если стадия 0, значит игрок еще не поговорил с зомби
        if (currentQuestStage == 0)
        {
            Debug.Log("Сначала нужно поговорить с зомби и взять задание!");
            // Тут можно показать сообщение на экране игроку, но пока просто в консоль
            return;
        }

        // 2. Если квест взят (стадия 1), проверяем доски
        InventorySystem inv = FindFirstObjectByType<InventorySystem>();

        if (inv != null && inv.HasItem(ContextKey.Item_Boards, boardCount))
        {
            miniGamePanel.SetActive(true);
            playerMovement.canMove = false; // Блокируем игрока
        }
        else
        {
            Debug.Log("Не хватает досок!");
        }
    }

    public void OnMinigameCompleted()
    {
        miniGamePanel.SetActive(false);
        playerMovement.canMove = true;

        // Ставим квест "выполнен"
        ContextBootstrapper.ContextSystem.TrySetState(ContextKey.Quest_ZombieFenceStarted, 2);

        // Тратим доски
        InventorySystem inv = FindFirstObjectByType<InventorySystem>();
        if (inv != null) inv.RemoveItem(ContextKey.Item_Boards, boardCount);
    }

    // Этот метод нужен для PlayerInteraction, чтобы панель "E" не появлялась, если мы еще не говорили с зомби
    public bool CanInteract()
    {
        int currentQuestStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Quest_ZombieFenceStarted, out int state))
        {
            currentQuestStage = state;
        }

        // Возвращаем true (можно взаимодействовать), только если квест уже взят (стадия 1 или 2)
        return currentQuestStage > 0;
    }
}