using SnogDialogue.Runtime;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private NPCInteractable currentNPC;
    private ItemPickup currentItem;
    private FenceRepairSpot currentFence;

    [SerializeField] private GameObject interactPromptUI;
    [SerializeField] private PlayerMovement playerMovement;

    private MultiNPCDialogue currentMultiNPC;

    private DialogueRunner runner;

    private void Start()
    {
        //runner = FindFirstObjectByType<DialogueRunner>();
        //if (runner != null)
        //{
        //    runner.OnDialogueStarted.AddListener(() => playerMovement.canMove = false);
        //    runner.OnDialogueFinished.AddListener(() => playerMovement.canMove = true);
        //}
        runner = FindFirstObjectByType<DialogueRunner>();
        if (runner != null && playerMovement != null)
        {
            runner.OnDialogueStarted.AddListener(() =>
            {
                playerMovement.canMove = false;
                playerMovement.canJump = false;   // <-- ДОБАВИТЬ
            });
            runner.OnDialogueFinished.AddListener(() =>
            {
                playerMovement.canMove = true;
                playerMovement.canJump = true;    // <-- ДОБАВИТЬ
            });
        }
    }

    public void TryInteract()
    {
        if (currentMultiNPC != null && currentMultiNPC.CanInteract())
        {
            currentMultiNPC.Interact();
            interactPromptUI.SetActive(false);
            return;
        }

        // 1. СНАЧАЛА ПРОВЕРЯЕМ ЗАБОР (он в приоритете)
        if (currentFence != null && currentFence.CanInteract())
        {
            currentFence.Interact();
            interactPromptUI.SetActive(false);
            return; // Выходим, чтобы не проверять других
        }

        // 2. Потом NPC
        if (currentNPC != null && currentNPC.CanInteract())
        {
            currentNPC.Interact();
            interactPromptUI.SetActive(false);
            return;
        }

        // 3. Потом предметы
        if (currentItem != null)
        {
            currentItem.Interact();
            interactPromptUI.SetActive(false);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MultiNPCDialogue multiNpc = other.GetComponent<MultiNPCDialogue>();
        if (multiNpc != null)
        {
            currentMultiNPC = multiNpc;
            interactPromptUI.SetActive(multiNpc.CanInteract());
            return;
        }

        // Сначала проверяем забор
        FenceRepairSpot fence = other.GetComponent<FenceRepairSpot>();
        if (fence != null)
        {
            currentFence = fence;
            // Показываем панель ТОЛЬКО если забор разрешает взаимодействие
            interactPromptUI.SetActive(fence.CanInteract());
            return; // Выходим, чтобы не искать других
        }

        // Потом NPC
        NPCInteractable npc = other.GetComponent<NPCInteractable>();
        if (npc != null)
        {
            currentNPC = npc;
            interactPromptUI.SetActive(npc.CanInteract());
            return;
        }

        // Потом предметы
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null)
        {
            currentItem = item;
            interactPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MultiNPCDialogue multiNpc = other.GetComponent<MultiNPCDialogue>();
        if (multiNpc != null && multiNpc == currentMultiNPC)
        {
            currentMultiNPC = null;
            interactPromptUI.SetActive(false);
        }

        // Проверяем забор
        FenceRepairSpot fence = other.GetComponent<FenceRepairSpot>();
        if (fence != null && fence == currentFence)
        {
            currentFence = null;
            interactPromptUI.SetActive(false);
        }

        // Проверяем NPC
        NPCInteractable npc = other.GetComponent<NPCInteractable>();
        if (npc != null && npc == currentNPC)
        {
            currentNPC = null;
            interactPromptUI.SetActive(false);
        }

        // Проверяем предметы
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null && item == currentItem)
        {
            currentItem = null;
            interactPromptUI.SetActive(false);
        }
    }
}