using SnogDialogue.Runtime;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private NPCInteractable currentNPC;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        NPCInteractable npc = other.GetComponent<NPCInteractable>();
        if (npc != null)
        {
            currentNPC = npc;
            // Показываем панель, только если NPC разрешает взаимодействие
            interactPromptUI.SetActive(npc.CanInteract());
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
    }
}