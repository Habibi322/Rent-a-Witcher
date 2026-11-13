using UnityEngine;

public class SimpleSocket : MonoBehaviour
{
    [Header("Настройки сокета")]
    [SerializeField] private string expectedBoardId = "DefaultBoard";

    public bool IsOccupied { get; private set; } = false;
    public string BoardId => expectedBoardId;

    public void Occupy(SimpleBoardDrag board)
    {
        if (IsOccupied) return;

        board.AttachTo(this);
        IsOccupied = true;

        CheckIfAllFilled();
    }

    private void CheckIfAllFilled()
    {
        SimpleSocket[] allSockets = FindObjectsByType<SimpleSocket>(FindObjectsSortMode.None);
        bool allFilled = true;

        foreach (var socket in allSockets)
        {
            if (!socket.IsOccupied)
            {
                allFilled = false;
                break;
            }
        }

        if (allFilled)
        {
            Debug.Log("Все доски на своих местах!");
            FenceRepairSpot spot = FindFirstObjectByType<FenceRepairSpot>();
            if (spot != null) spot.OnMinigameCompleted();
        }
    }
}