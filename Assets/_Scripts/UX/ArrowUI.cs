using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    [SerializeField] private GameObject arrowUI;
    [SerializeField] private ContextKey questKey = ContextKey.Quest_ZombieFenceStarted;

    private void Start()
    {
        if (arrowUI != null) arrowUI.SetActive(false);

        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.SubscribeToContext(questKey, OnQuestStageChanged, true);
        }
    }

    private void OnDestroy()
    {
        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(questKey, OnQuestStageChanged);
        }
    }

    private void OnQuestStageChanged(int stage)
    {
        if (arrowUI != null) arrowUI.SetActive(stage == 2);
    }

    // НОВЫЙ МЕТОД: Включает стрелку вручную
    public void ShowArrow()
    {
        if (arrowUI != null) arrowUI.SetActive(true);
    }
}