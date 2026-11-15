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

    [Header("Стрелка (только для этого НПС)")]
    [SerializeField] private ArrowManager arrowManager; // <--- НОВОЕ

    [Header("Контекст")]
    [SerializeField] private ContextKey questStageKey = ContextKey.Quest_ZombieFenceStarted;

    private DialogueGraph _currentGraph; // Запоминаем, какой граф сейчас играет

    private void Start()
    {
        // Подписываемся на окончание диалога
        if (runner != null)
        {
            runner.OnDialogueFinished.AddListener(OnDialogueFinished);
        }
    }

    private void OnDestroy()
    {
        if (runner != null)
        {
            runner.OnDialogueFinished.RemoveListener(OnDialogueFinished);
        }
    }

    public void Interact()
    {
        if (runner == null) return;

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
                    _currentGraph = firstDialogueGraph; // Запоминаем граф
                    runner.Play(firstDialogueGraph);
                    ContextBootstrapper.ContextSystem.TrySetState(questStageKey, 1);
                }
                break;
            case 1:
                if (secondDialogueGraph != null)
                {
                    _currentGraph = secondDialogueGraph;
                    runner.Play(secondDialogueGraph);
                }
                break;
            case 2:
                if (thirdDialogueGraph != null)
                {
                    _currentGraph = thirdDialogueGraph; // Запоминаем третий граф
                    runner.Play(thirdDialogueGraph);
                }
                break;
        }
    }

    // Вызывается, когда ЛЮБОЙ диалог завершается
    private void OnDialogueFinished()
    {
        // Проверяем, что завершился именно ТРЕТИЙ диалог
        if (_currentGraph == thirdDialogueGraph && thirdDialogueGraph != null)
        {
            Debug.Log("Третий диалог завершен! Включаем стрелку.");

            // Включаем стрелку
            if (arrowManager != null)
            {
                arrowManager.ShowArrow();
            }

            // Чтобы не сработало дважды, можно отписаться
            // runner.OnDialogueFinished.RemoveListener(OnDialogueFinished);
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}