using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleSocket : MonoBehaviour, IDropHandler
{
    public bool IsOccupied { get; private set; } = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (IsOccupied) return;

        // Получаем объект, который перетащили
        SimpleBoardDrag draggedBoard = eventData.pointerDrag.GetComponent<SimpleBoardDrag>();

        if (draggedBoard != null)
        {
            // Примагничиваем доску к центру сокета
            draggedBoard.SnapTo(GetComponent<RectTransform>().anchoredPosition);
            IsOccupied = true;

            CheckIfAllFilled();
        }
        else
        {
            // Если отпустили не доску, возвращаем её на место
            SimpleBoardDrag board = eventData.pointerDrag.GetComponent<SimpleBoardDrag>();
            if (board != null) board.ReturnToStart();
        }
    }

    private void CheckIfAllFilled()
    {
        SimpleSocket[] allSockets = FindObjectsByType<SimpleSocket>(FindObjectsSortMode.None);
        bool allFilled = true;

        foreach (var socket in allSockets)
        {
            if (!socket.IsOccupied) { allFilled = false; break; }
        }

        if (allFilled)
        {
            Debug.Log("Все доски на месте!");
            FindFirstObjectByType<FenceRepairSpot>().OnMinigameCompleted();
        }
    }
}