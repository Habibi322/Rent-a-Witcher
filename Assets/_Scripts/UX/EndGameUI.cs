using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class EndGameUI : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text endGameText;

    [Header("Настройки перезапуска")]
    [Tooltip("Имя сцены главного меню для загрузки после заставки")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Tooltip("Задержка перед загрузкой меню (в секундах)")]
    [SerializeField] private float delayBeforeMenu = 3f;

    private void Start()
    {
        if (endGamePanel != null) endGamePanel.SetActive(false);

        if (ContextBootstrapper.ContextSystem != null)
        {
            ContextBootstrapper.ContextSystem.SubscribeToContext(
                ContextKey.Quest_PipesStage, OnQuestStageChanged, true
            );
        }
    }

    private void OnDestroy()
    {
        if (ContextBootstrapper.ContextSystem != null)
            ContextBootstrapper.ContextSystem.UnsubscribeFromContext(ContextKey.Quest_PipesStage, OnQuestStageChanged);
    }

    private void OnQuestStageChanged(int stage)
    {
        // Когда стадия достигает 4 (финал)
        if (stage >= 4)
        {
            if (endGamePanel != null) endGamePanel.SetActive(true);
            if (endGameText != null)
            {
                endGameText.text = "Спасибо, что сыграли в нашу игру.\nПродолжение будет... возможно... мы постараемся :3";
            }

            // Запускаем корутину для перехода в меню
            StartCoroutine(ReturnToMenuRoutine());
        }
    }

    private IEnumerator ReturnToMenuRoutine()
    {
        // Ждем, пока игрок прочитает текст
        yield return new WaitForSeconds(delayBeforeMenu);

        // Возвращаем время в норму (на случай, если была пауза)
        Time.timeScale = 1f;

        // Загружаем главное меню
        SceneManager.LoadScene(mainMenuSceneName);
    }
}