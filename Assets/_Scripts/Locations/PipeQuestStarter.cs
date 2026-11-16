using UnityEngine;

public class PipeQuestStarter : MonoBehaviour
{
    [SerializeField] private ContextKey questStageKey = ContextKey.Quest_PipesStage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Проверяем, не запущен ли уже квест
            int currentStage = 0;
            if (ContextBootstrapper.ContextSystem.TryGetState(questStageKey, out int s))
                currentStage = s;

            if (currentStage == 0)
            {
                // Запускаем квест
                ContextBootstrapper.ContextSystem.TrySetState(questStageKey, 1);
                Debug.Log("Квест 'Трубы горят' начат!");
            }
        }
    }
}