using SnogDialogue.Runtime;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [Header("Графы диалогов")]
    [SerializeField] private DialogueGraph firstDialogueGraph; // Для первого раза
    [SerializeField] private DialogueGraph secondDialogueGraph; // Для второго раза (после квеста)

    [Header("Ссылки")]
    [SerializeField] private DialogueRunner runner;

    [Header("Context Settings")]
    [Tooltip("Какой ключ выставить в 1 после первого диалога")]
    [SerializeField] private ContextKey dialogFinishedKey;

    public void Interact()
    {
        if (runner == null) return;

        // Проверяем, говорили ли мы уже
        bool hasTalked = false;
        if (ContextBootstrapper.ContextSystem.TryGetState(dialogFinishedKey, out int value))
        {
            hasTalked = (value > 0);
        }

        if (hasTalked && secondDialogueGraph != null)
        {
            runner.Play(secondDialogueGraph);
        }
        else if (!hasTalked && firstDialogueGraph != null)
        {
            // Запускаем первый диалог
            runner.Play(firstDialogueGraph);

            // Записываем в контекст, что поговорили
            ContextBootstrapper.ContextSystem.TrySetState(dialogFinishedKey, 1);
        }
    }

    // Метод для PlayerInteraction, чтобы знать, показывать ли "E"
    public bool CanInteract()
    {
        if (ContextBootstrapper.ContextSystem.TryGetState(dialogFinishedKey, out int value))
        {
            return value == 0; // Если 0 - можно, если 1 - уже поговорили
        }
        return true;
    }
}