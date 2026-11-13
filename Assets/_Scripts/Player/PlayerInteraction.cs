using SnogDialogue.Runtime;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private NPCInteractable currentNPC;
    private ItemPickup currentItem;

    [SerializeField] private GameObject interactPromptUI;
    [SerializeField] private PlayerMovement playerMovement;

    private DialogueRunner runner;

    private void Start()
    {
        runner = FindFirstObjectByType<DialogueRunner>();
        if (runner != null)
        {
            runner.OnDialogueStarted.AddListener(() => playerMovement.canMove = false);
            runner.OnDialogueFinished.AddListener(() => playerMovement.canMove = true);
        }
    }

    public void TryInteract()
    {
        if (currentNPC != null && currentNPC.CanInteract())
        {
            currentNPC.Interact();
            interactPromptUI.SetActive(false);
        }
        else if (currentItem != null)
        {
            currentItem.Interact();
            interactPromptUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        NPCInteractable npc = other.GetComponent<NPCInteractable>();
        if (npc != null)
        {
            currentNPC = npc;
            interactPromptUI.SetActive(npc.CanInteract());
            return;
        }

        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null)
        {
            currentItem = item;
            interactPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NPCInteractable npc = other.GetComponent<NPCInteractable>();
        if (npc != null && npc == currentNPC)
        {
            currentNPC = null;
            interactPromptUI.SetActive(false);
        }

        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null && item == currentItem)
        {
            currentItem = null;
            interactPromptUI.SetActive(false);
        }
    }
}