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
    [SerializeField] private float delayBeforeMenu = 5f;

    [Tooltip("Время плавного появления (в секундах)")]
    [SerializeField] private float fadeDuration = 2f;

    private CanvasGroup panelCanvasGroup;
    private CanvasGroup textCanvasGroup;

    private void Start()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
            panelCanvasGroup = endGamePanel.GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
                panelCanvasGroup = endGamePanel.AddComponent<CanvasGroup>();
            panelCanvasGroup.alpha = 0f;
        }

        if (endGameText != null)
        {
            textCanvasGroup = endGameText.GetComponent<CanvasGroup>();
            if (textCanvasGroup == null)
                textCanvasGroup = endGameText.gameObject.AddComponent<CanvasGroup>();
            textCanvasGroup.alpha = 0f;
        }

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
        if (stage >= 4)
        {
            StartCoroutine(ShowEndGameRoutine());
        }
    }

    private IEnumerator ShowEndGameRoutine()
    {
        // 1. Включаем панель (но она прозрачная)
        if (endGamePanel != null)
            endGamePanel.SetActive(true);

        // 2. Плавно показываем текст
        if (endGameText != null)
        {
            endGameText.text = "Спасибо, что сыграли в нашу игру.\nПродолжение будет... возможно... мы постараемся :3";
        }

        // 3. Плавное появление панели (затемнение фона)
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            if (panelCanvasGroup != null)
                panelCanvasGroup.alpha = alpha;

            yield return null;
        }
        if (panelCanvasGroup != null)
            panelCanvasGroup.alpha = 1f;

        // 4. Плавное появление текста (с небольшой задержкой)
        yield return new WaitForSeconds(0.5f);

        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            if (textCanvasGroup != null)
                textCanvasGroup.alpha = alpha;

            yield return null;
        }
        if (textCanvasGroup != null)
            textCanvasGroup.alpha = 1f;

        // 5. Ждем, пока игрок прочитает текст
        yield return new WaitForSeconds(delayBeforeMenu);

        // 6. Возвращаем время в норму и загружаем меню
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}