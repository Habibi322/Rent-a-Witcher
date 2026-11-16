using UnityEngine;
using System.Collections;

public class BiomeTransition : MonoBehaviour
{
    [Header("Настройки перехода")]
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private float fadeDuration = 2.5f; // Время полного затемнения

    [Header("Ссылки")]
    [SerializeField] private PlayerMovement playerMovement; // <--- НОВОЕ: Ссылка на движение игрока

    private bool isTransitioning = false;
    private CanvasGroup fadeCanvasGroup;

    private void Start()
    {
        if (fadePanel != null)
        {
            fadeCanvasGroup = fadePanel.GetComponent<CanvasGroup>();
            if (fadeCanvasGroup == null)
                fadeCanvasGroup = fadePanel.AddComponent<CanvasGroup>();

            // Изначально экран прозрачный
            fadeCanvasGroup.alpha = 0f;
            fadePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning) return;

        if (other.CompareTag("Player"))
        {
            int currentStage = 0;
            if (ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Quest_ZombieFenceStarted, out int state))
                currentStage = state;

            if (currentStage == 3)
            {
                StartCoroutine(TransitionRoutine(other.transform));
            }
        }
    }

    private IEnumerator TransitionRoutine(Transform playerTransform)
    {
        isTransitioning = true;

        // 1. Блокируем движение игрока
        if (playerMovement != null)
            playerMovement.canMove = false;

        // Отключаем стрелку
        ArrowManager arrow = FindFirstObjectByType<ArrowManager>();
        if (arrow != null)
        {
            GameObject arrowObj = GameObject.Find("ArrowUI"); // Замените на имя вашего объекта стрелки
            if (arrowObj != null) arrowObj.SetActive(false);
        }

        // 2. Плавное затемнение
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        // 3. Телепортация
        if (teleportTarget != null)
        {
            Vector3 newPosition = teleportTarget.position;
            newPosition.y = playerTransform.position.y;
            playerTransform.position = newPosition;
        }

        // 4. Плавное осветление
        if (fadePanel != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 0f;
            fadePanel.SetActive(false);
        }

        // 5. Разблокируем движение игрока ПОСЛЕ осветления
        if (playerMovement != null)
            playerMovement.canMove = true;

        isTransitioning = false;
        this.enabled = false;
    }
}