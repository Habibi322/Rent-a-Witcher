using SnogDialogue.Runtime;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [Header("Графы диалогов")]
    [SerializeField] private DialogueGraph firstDialogueGraph;  // 0: До квеста
    [SerializeField] private DialogueGraph secondDialogueGraph; // 1: В процессе
    [SerializeField] private DialogueGraph thirdDialogueGraph;  // 2: Завершено

    [Header("Ссылки")]
    [SerializeField] private DialogueRunner runner;

    [Header("Контекст")]
    [Tooltip("Ключ, который отслеживает стадию диалога/квеста")]
    [SerializeField] private ContextKey questStageKey = ContextKey.Quest_ZombieFenceStarted;

    public void Interact()
    {
        if (runner == null) return;

        // Получаем текущую стадию из вашей системы
        int currentStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(questStageKey, out int state))
        {
            currentStage = state;
        }

        switch (currentStage)
        {
            case 0:
                if (firstDialogueGraph != null)
                {
                    runner.Play(firstDialogueGraph);
                    // После первого разговора переводим стадию на 1 (Задание взято)
                    ContextBootstrapper.ContextSystem.TrySetState(questStageKey, 1);
                }
                break;
            case 1:
                if (secondDialogueGraph != null)
                    runner.Play(secondDialogueGraph);
                break;
            case 2:
                if (thirdDialogueGraph != null)
                    runner.Play(thirdDialogueGraph);
                break;
        }
    }

    // Метод для PlayerInteraction, чтобы решить, показывать ли "E"
    public bool CanInteract()
    {
        // В этой логике мы разрешаем говорить всегда (пока вы не захотите запретить)
        return true;
    }
}