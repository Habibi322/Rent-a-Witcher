using UnityEngine;

public class QPrompt : MonoBehaviour
{
    [SerializeField] private GameObject qPromptUI; // —сылка на UI с буквой Q

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"QPrompt: триггер сработал на {other.name}, тег: {other.tag}");
        if (other.CompareTag("Player"))
        {
            if (qPromptUI != null)
            {
                qPromptUI.SetActive(true);
                Debug.Log($"QPrompt: включен {qPromptUI.name}, активен: {qPromptUI.activeSelf}");
            }
            else
            {
                Debug.LogError("QPrompt: qPromptUI равен null!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (qPromptUI != null) qPromptUI.SetActive(false);
        }
    }
}