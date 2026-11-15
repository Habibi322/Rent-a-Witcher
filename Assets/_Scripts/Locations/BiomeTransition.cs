using UnityEngine;
using System.Collections;

public class BiomeTransition : MonoBehaviour
{
    [Header("Настройки перехода")]
    [SerializeField] private Transform teleportTarget; // Точка, куда телепортируется игрок
    [SerializeField] private GameObject fadePanel;      // Черный экран (UI)
    [SerializeField] private float fadeDuration = 2.5f; // Время затемнения

    private bool isTransitioning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning) return;

        // Проверяем, что это игрок и что квест выполнен
        if (other.CompareTag("Player"))
        {
            int currentStage = 0;
            if (ContextBootstrapper.ContextSystem.TryGetState(ContextKey.Quest_ZombieFenceStarted, out int state))
            {
                currentStage = state;
            }

            if (currentStage == 2)
            {
                StartCoroutine(TransitionRoutine(other.transform));
            }
        }
    }

    private IEnumerator TransitionRoutine(Transform playerTransform)
    {
        isTransitioning = true;

        // 1. Включаем затемнение
        if (fadePanel != null) fadePanel.SetActive(true);

        // 2. Ждем, пока экран потемнеет
        yield return new WaitForSeconds(fadeDuration);

        // 3. Телепортируем игрока (по Y оставляем как есть)
        if (teleportTarget != null)
        {
            Vector3 newPosition = teleportTarget.position;
            newPosition.y = playerTransform.position.y; // Сохраняем высоту
            playerTransform.position = newPosition;
        }

        // 4. Выключаем затемнение
        if (fadePanel != null) fadePanel.SetActive(false);

        isTransitioning = false;

        // Отключаем триггер, чтобы не сработал дважды
        this.enabled = false;
    }
}