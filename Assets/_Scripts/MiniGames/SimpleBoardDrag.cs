using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleBoardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Настройки детали")]
    [SerializeField] private string boardId = "DefaultBoard";

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;
    private bool isSnapped = false;

    public string BoardId => boardId;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSnapped = false;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        SimpleSocket target = FindSocketUnderBoard();

        if (target != null)
        {

            target.Occupy(this);
            isSnapped = true;
        }
        else
        {
            ReturnToStart();
        }
    }

    private SimpleSocket FindSocketUnderBoard()
    {
        SimpleSocket[] sockets = FindObjectsByType<SimpleSocket>(FindObjectsSortMode.None);

        Vector2 boardScreenPos = RectTransformUtility.WorldToScreenPoint(
            GetCanvasCamera(), rectTransform.position);

        foreach (var socket in sockets)
        {
            if (socket.IsOccupied) continue;
            if (socket.BoardId != boardId) continue;

            RectTransform socketRect = socket.GetComponent<RectTransform>();

            if (RectTransformUtility.RectangleContainsScreenPoint(
                    socketRect, boardScreenPos, GetCanvasCamera()))
            {
                return socket;
            }
        }

        return null;
    }

    private Camera GetCanvasCamera()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return null;

        Canvas root = canvas.rootCanvas;
        return root.renderMode == RenderMode.ScreenSpaceOverlay ? null : root.worldCamera;
    }

    public void AttachTo(SimpleSocket socket)
    {
        transform.SetParent(socket.transform, false);
        rectTransform.anchoredPosition = Vector2.zero;

        isSnapped = true;
        this.enabled = false;
    }

    public void ReturnToStart()
    {
        rectTransform.anchoredPosition = startPosition;
    }
}