using SnogDialogue.Runtime;
using UnityEngine;

public class NPCDialogueAudio : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private DialogueRunner runner;          // тот же runner, что у NPCInteractable

    [Header("Звуковые объекты NPC")]
    [Tooltip("Объекты/компоненты, которые должны звучать только в диалоге. " +
             "Можно перетащить GameObject со звуком (например, 'ZombieVoice') " +
             "или AudioSource напрямую.")]
    [SerializeField] private GameObject[] soundObjects;       // или AudioSource[] — см. ниже

    private void Start()
    {
        if (runner == null) return;
        runner.OnDialogueStarted.AddListener(OnDialogueStarted);
        runner.OnDialogueFinished.AddListener(OnDialogueFinished);

        SetSoundActive(false); // по умолчанию тихо
    }

    private void OnDestroy()
    {
        if (runner == null) return;
        runner.OnDialogueStarted.RemoveListener(OnDialogueStarted);
        runner.OnDialogueFinished.RemoveListener(OnDialogueFinished);
    }

    private void OnDialogueStarted()
    {
        SetSoundActive(true);
    }

    private void OnDialogueFinished()
    {
        SetSoundActive(false);
    }

    private void SetSoundActive(bool active)
    {
        if (soundObjects == null) return;
        foreach (var go in soundObjects)
        {
            if (go != null) go.SetActive(active);
        }
    }
}