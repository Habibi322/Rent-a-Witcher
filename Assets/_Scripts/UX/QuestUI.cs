using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questDescriptionText;

    private void Start()
    {
        if (ContextBootstrapper.ContextSystem != null)
        {
            // Подписка на квест с забором
            ContextBootstrapper.ContextSystem.SubscribeToContext(ContextKey.Quest_ZombieFenceStarted, OnQuestChanged, true);
            ContextBootstrapper.ContextSystem.SubscribeToContext(ContextKey.Item_Boards, OnQuestChanged, true);

            // Подписка на квест с трубами
            ContextBootstrapper.ContextSystem.SubscribeToContext(ContextKey.Quest_PipesStage, OnQuestChanged, true);
            ContextBootstrapper.ContextSystem.SubscribeToContext(ContextKey.Quest_NewBiomeTalked_1, OnQuestChanged, true);
            ContextBootstrapper.ContextSystem.SubscribeToContext(ContextKey.Quest_NewBiomeTalked_2, OnQuestChanged, true);
            ContextBootstrapper.ContextSystem.SubscribeToContext(ContextKey.Quest_NewBiomeTalked_3, OnQuestChanged, true);

        }
    }

    private void OnDestroy()
    {
        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Quest_ZombieFenceStarted, OnQuestChanged);
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Item_Boards, OnQuestChanged);
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Quest_PipesStage, OnQuestChanged);
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Quest_NewBiomeTalked_1, OnQuestChanged);
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Quest_NewBiomeTalked_2, OnQuestChanged);
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Quest_NewBiomeTalked_3, OnQuestChanged);
            
        }
    }

    private void OnQuestChanged(int value) => UpdateQuestUI();

    private void UpdateQuestUI()
    {
        // --- КВЕСТ 1: ЗАБОР ---
        int fenceStage = 0;
        ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Quest_ZombieFenceStarted, out int fs);
        fenceStage = fs;

        if (fenceStage > 0 && fenceStage < 3)
        {
            questPanel.SetActive(true);
            questTitleText.text = "ЗАБОР ВОЙНЫ";

            int boards = 0;
            ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Item_Boards, out int b);
            boards = b;

            if (fenceStage == 1)
                questDescriptionText.text = (boards < 6) ? "Соберите доски" : "Почините забор";
            else if (fenceStage == 2)
                questDescriptionText.text = "Поговорите с существами";
            return;
        }

        // --- КВЕСТ 2: ТРУБЫ ---
        int pipeStage = 0;
        ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Quest_PipesStage, out int ps);
        pipeStage = ps;

        if (pipeStage > 0 && pipeStage < 4) // <--- Теперь стадии до 4
        {
            questPanel.SetActive(true);
            questTitleText.text = "ТРУБЫ ГОРЯТ";

            if (pipeStage == 1)
            {
                questDescriptionText.text = "Узнайте, что произошло";
            }
            else if (pipeStage == 2)
            {
                questDescriptionText.text = "Сдвиньте ящера и почините трубу";
            }
            else if (pipeStage == 3)
            {
                questDescriptionText.text = "Поговорите с ближайшим существом";
            }
            return;
        }

        // Если ни один квест не активен — скрываем панель
        questPanel.SetActive(false);
    }
}