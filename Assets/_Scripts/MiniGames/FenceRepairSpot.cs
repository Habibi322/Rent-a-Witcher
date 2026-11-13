using UnityEngine;

public class FenceRepairSpot : MonoBehaviour
{
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private int boardCount;

    public void Interact()
    {
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
        ContextBootstrapper.ContextSystem.TrySetState(ContextKey.Quest_ZombieFenceCompleted, 2);

        // Тратим доски
        InventorySystem inv = FindFirstObjectByType<InventorySystem>();
        if (inv != null) inv.RemoveItem(ContextKey.Item_Boards, boardCount);
    }
}