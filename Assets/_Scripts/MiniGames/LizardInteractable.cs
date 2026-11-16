using UnityEngine;

public class LizardInteractable : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private ContextKey questStageKey = ContextKey.Quest_PipesStage;

    private bool _hasMoved = false;

    // Этот метод мы вызовем из AardManager, когда ящер будет схвачен
    public void OnAardGrabbed()
    {
        if (_hasMoved) return;

        // Проверяем стадию квеста (1 - ищем ящера)
        int currentStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(questStageKey, out int s))
            currentStage = s;

        if (currentStage == 1) // Только на стадии "Сдвиньте ящера"
        {
            _hasMoved = true;

            // Переводим квест на стадию 2 (ящер сдвинут, труба ждет)
            ContextBootstrapper.ContextSystem.TrySetState(questStageKey, 2);
            Debug.Log("Ящер сдвинут! Теперь почините трубу.");
        }
    }
}