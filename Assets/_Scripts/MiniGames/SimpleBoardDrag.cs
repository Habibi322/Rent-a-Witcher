using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleBoardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Делаем доску полупрозрачной при захвате
        canvasGroup.blocksRaycasts = false; // Чтобы мышь "проходила" сквозь доску
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta; // Двигаем за мышкой
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Если доска не прилипла к сокету (сокет не вызвал SnapTo), возвращаем её на место
        // (Проверка происходит внутри сокета через OnDrop)
    }

    // Вызывается из сокета, когда доска попадает в цель
    public void SnapTo(Vector2 position)
    {
        rectTransform.anchoredPosition = position;
        this.enabled = false; // Отключаем скрипт, чтобы доску нельзя было таскать снова
    }

    // Возврат на место, если не попали в сокет
    public void ReturnToStart()
    {
        rectTransform.anchoredPosition = startPosition;
    }
}