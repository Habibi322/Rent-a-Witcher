using SnogDialogue.Runtime;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [Header("Графы диалогов")]
    [SerializeField] private DialogueGraph firstDialogueGraph;
    [SerializeField] private DialogueGraph secondDialogueGraph;
    [SerializeField] private DialogueGraph thirdDialogueGraph;

    [Header("Ссылки")]
    [SerializeField] private DialogueRunner runner;
    [SerializeField] private ArrowManager arrowManager;

    [Header("Визуал зомби")]
    [SerializeField] private SpriteRenderer[] zombieRenderers; // Сюда перетащите SpriteRenderer дочерних зомби
    [SerializeField] private SpriteRenderer parentRenderer;    // Сюда перетащите SpriteRenderer родителя

    [Header("Контекст")]
    [SerializeField] private ContextKey questStageKey = ContextKey.Quest_ZombieFenceStarted;

    private DialogueGraph _currentGraph;

    private void Start()
    {
        if (runner != null)
            runner.OnDialogueFinished.AddListener(OnDialogueFinished);

        // Подписываемся на изменение стадии квеста, чтобы менять спрайты
        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.SubscribeToContext(
                questStageKey, OnQuestStageChanged, true
            );
        }
    }

    private void OnDestroy()
    {
        if (runner != null)
            runner.OnDialogueFinished.RemoveListener(OnDialogueFinished);

        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(questStageKey, OnQuestStageChanged);
        }
    }

    // Этот метод вызывается при изменении стадии квеста
    private void OnQuestStageChanged(int stage)
    {
        bool showParent = (stage >= 2); // Если забор починен (стадия 2), показываем родителя

        // Выключаем дочерние спрайты
        foreach (var sr in zombieRenderers)
        {
            if (sr != null) sr.enabled = !showParent;
        }

        // Включаем родительский спрайт
        if (parentRenderer != null)
            parentRenderer.enabled = showParent;
    }

    public void Interact()
    {
        if (runner == null) return;

        int currentStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(questStageKey, out int state))
            currentStage = state;

        switch (currentStage)
        {
            case 0:
                if (firstDialogueGraph != null)
                {
                    _currentGraph = firstDialogueGraph;
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
                    _currentGraph = thirdDialogueGraph;
                    runner.Play(thirdDialogueGraph);
                }
                break;
        }
    }

    private void OnDialogueFinished()
    {
        // Проверяем, что завершился именно ТРЕТИЙ диалог
        if (_currentGraph == thirdDialogueGraph && thirdDialogueGraph != null)
        {
            Debug.Log("Третий диалог завершен! Включаем стрелку и скрываем квест.");

            // Включаем стрелку
            //if (arrowManager != null) arrowManager.ShowArrow();

            // Ставим стадию 3, чтобы панель квестов поняла, что всё выполнено
            ContextBootstrapper.ContextSystem.TrySetState(questStageKey, 3);
        }
    }

    public bool CanInteract() => true;
}