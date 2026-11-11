using UnityEngine;

public class Pipe : MonoBehaviour, IIgnitable
{
    [Header("Настройки трубы")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite weldedSprite;

    private bool isWelded = false;

    public bool Ignite(float duration)
    {
        if (isWelded)
        {
            return false;
        }

        if (spriteRenderer != null && weldedSprite != null)
        {
            spriteRenderer.sprite = weldedSprite;
        }

        isWelded = true;
        return true;
    }
}