using UnityEngine;
using TMPro; // Для TextMeshPro

public class FenceRepairSpot : MonoBehaviour
{
    [Header("Мини-игра")]
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private int boardCount = 6; // Нужно 6 досок

    [Header("Текст подсказки")]
    [SerializeField] private TMP_Text interactionText; // Сюда перетащите ваш текстовый объект

    public void Interact()
    {
        // Получаем стадию квеста (0 - не говорили, 1 - говорили)
        int currentStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Quest_ZombieFenceStarted, out int state))
        {
            currentStage = state;
        }

        // Получаем количество досок
        int currentBoards = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Item_Boards, out int boards))
        {
            currentBoards = boards;
        }

        // --- ЛОГИКА ТЕКСТОВ ---

        // 1.1 и 1.2: Еще не говорили с зомби (стадия 0)
        if (currentStage == 0)
        {
            if (currentBoards >= boardCount)
            {
                ShowText("Лучше не трогать. Сначала поговорю с теми существами"); // 1.1
            }
            else
            {
                ShowText("Сначала поговорю с теми существами"); // 1.2
            }
            return;
        }

        // 1.3 и 1.4: Уже поговорили (стадия 1)
        if (currentStage >= 1)
        {
            if (currentBoards == 0)
            {
                ShowText("Мне сначала нужны доски. Шести должно хватить"); // 1.3
            }
            else if (currentBoards < boardCount)
            {
                ShowText("Не хватает досок"); // 1.4
            }
            else if (currentBoards >= boardCount)
            {
                // Если досок хватает — открываем мини-игру
                miniGamePanel.SetActive(true);
                playerMovement.canMove = false;
            }
        }
    }

    // Вспомогательный метод для показа текста
    private void ShowText(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
            interactionText.gameObject.SetActive(true);
            // Можно добавить таймер, чтобы текст исчезал через 3 секунды
            Invoke(nameof(HideText), 3f);
        }
    }

    private void HideText()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    public void OnMinigameCompleted()
    {
        miniGamePanel.SetActive(false);
        playerMovement.canMove = true;

        ContextBootstrapper.ContextSystem.TrySetState(ContextKey.Quest_ZombieFenceStarted, 2);

        InventorySystem inv = FindFirstObjectByType<InventorySystem>();
        if (inv != null) inv.RemoveItem(ContextKey.Item_Boards, boardCount);
    }

    public bool CanInteract()
    {
        return true; // Разрешаем нажимать E всегда, чтобы увидеть текст
    }
}