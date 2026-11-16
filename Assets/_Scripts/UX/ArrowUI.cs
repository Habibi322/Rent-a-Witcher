using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    [Header("UI Стрелка")]
    [SerializeField] private GameObject arrowUI;

    [Header("Квест: Забор")]
    [SerializeField] private ContextKey questKey = ContextKey.Quest_ZombieFenceStarted;
    [SerializeField] private int arrowStage = 3;

    [Header("Новый биом")]
    [SerializeField] private ContextKey newBiomeKey = ContextKey.Quest_PipesStage;

    private void Start()
    {
        if (arrowUI != null) arrowUI.SetActive(false);

        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.SubscribeToContext(questKey, OnQuestStageChanged, true);
            ContextBootstrapper.ContextSystem.SubscribeToContext(newBiomeKey, OnNewBiomeChanged, true);
        }
    }

    private void OnDestroy()
    {
        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(questKey, OnQuestStageChanged);
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(newBiomeKey, OnNewBiomeChanged);
        }
    }

    private void OnQuestStageChanged(int stage)
    {
        UpdateArrowVisibility();
    }

    private void OnNewBiomeChanged(int stage)
    {
        UpdateArrowVisibility();
    }

    private void UpdateArrowVisibility()
    {
        if (arrowUI == null) return;

        // Проверяем, не в новом ли мы биоме
        int newBiomeStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(newBiomeKey, out int nb))
            newBiomeStage = nb;

        // Если мы уже в новом биоме (стадия > 0), стрелку не показываем НИКОГДА
        if (newBiomeStage > 0)
        {
            arrowUI.SetActive(false);
            return;
        }

        // Если мы все еще в первом биоме, проверяем квест с забором
        int fenceStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(questKey, out int fs))
            fenceStage = fs;

        arrowUI.SetActive(fenceStage >= arrowStage);
    }

    public void ShowArrow()
    {
        if (arrowUI != null) arrowUI.SetActive(true);
    }
}