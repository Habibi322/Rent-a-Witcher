using UnityEngine;

public class Pipe : MonoBehaviour, IIgnitable
{
    [Header("Настройки трубы")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite weldedSprite;

    [Header("Контекст")]
    [SerializeField] private ContextKey questStageKey = ContextKey.Quest_PipesStage;

    private bool isWelded = false;

    public bool Ignite(float duration)
    {
        if (isWelded)
        {
            return false;
        }

        // Проверяем стадию квеста (2 - труба ждет починки)
        int currentStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(questStageKey, out int s))
            currentStage = s;

        if (currentStage != 2)
        {
            Debug.Log("Сейчас не время чинить трубу!");
            return false;
        }

        if (spriteRenderer != null && weldedSprite != null)
        {
            spriteRenderer.sprite = weldedSprite;
        }

        isWelded = true;

        // Переводим квест на стадию 3 (труба починена, можно говорить с 4-м NPC)
        ContextBootstrapper.ContextSystem.TrySetState(questStageKey, 3);
        Debug.Log("Труба починена! Поговорите с существом.");

        return true;
    }
}