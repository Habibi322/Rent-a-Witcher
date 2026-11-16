using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questDescriptionText;

    private void Start()
    {
        // Подписываемся на изменение стадии квеста
        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.SubscribeToContext(
                ContextKey.Quest_ZombieFenceStarted,
                OnQuestStageChanged,
                true
            );
            // Также подписываемся на изменение количества досок, чтобы обновлять описание
            ContextBootstrapper.ContextSystem.SubscribeToContext(
                ContextKey.Item_Boards,
                OnBoardsChanged,
                true
            );
        }
    }

    private void OnDestroy()
    {
        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Quest_ZombieFenceStarted, OnQuestStageChanged);
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Item_Boards, OnBoardsChanged);
        }
    }

    private void OnQuestStageChanged(int stage)
    {
        UpdateQuestUI();
    }

    private void OnBoardsChanged(int amount)
    {
        UpdateQuestUI();
    }

    private void UpdateQuestUI()
    {
        int stage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Quest_ZombieFenceStarted, out int s)) stage = s;

        int boards = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Item_Boards, out int b)) boards = b;

        // Если квест не взят — скрываем панель (или пишем "Нет заданий")
        if (stage == 0)
        {
            questTitleText.text = "";
            questDescriptionText.text = "";
            return;
        }

        // Если квест взят (стадия 1 и выше)
        questTitleText.text = "ЗАБОР ВОЙНЫ";

        // Логика смены пунктов
        if (stage == 1) // Квест в процессе
        {
            if (boards < 6)
            {
                // Если досок меньше 6 — показываем первый пункт
                questDescriptionText.text = "Соберите доски";
            }
            else
            {
                // Если досок 6 и больше — показываем второй пункт
                questDescriptionText.text = "Почините забор";
            }
        }
        else if (stage == 2) // Квест выполнен (забор починен)
        {
            questDescriptionText.text = "Поговорите с существами";
        }
    }
}