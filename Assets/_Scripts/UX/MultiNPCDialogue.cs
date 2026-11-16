using SnogDialogue.Runtime;
using UnityEngine;

public class MultiNPCDialogue : MonoBehaviour
{
    [Header("Ãğàôû äèàëîãîâ")]
    [SerializeField] private DialogueGraph uniqueDialogueGraph;   // Äèàëîã 1, 2, 3
    [SerializeField] private DialogueGraph fallbackDialogueGraph; // Äèàëîã 5 (åñëè íàğóøåí ïîğÿäîê)

    [Header("Ïîñëå ïî÷èíêè òğóáû")]
    [SerializeField] private DialogueGraph fourthNpcAfterRepairGraph; // Äèàëîã 6 (òîëüêî äëÿ 3-ãî NPC)
    [SerializeField] private DialogueGraph otherNpcAfterRepairGraph;  // Äèàëîã 7 (äëÿ 1, 2 NPC)

    [Header("Ññûëêè")]
    [SerializeField] private DialogueRunner runner;
    //[SerializeField] private ArrowManager arrowManager;

    [Header("Êîíòåêñò")]
    [SerializeField] private ContextKey myUniqueKey;
    [SerializeField] private int myOrderIndex;
    [SerializeField] private ContextKey previousNpcKey;
    [SerializeField] private ContextKey pipesQuestStageKey = ContextKey.Quest_PipesStage;

    private DialogueGraph _currentGraph;

    private void Start()
    {
        if (runner != null) runner.OnDialogueFinished.AddListener(OnDialogueFinished);
    }

    private void OnDestroy()
    {
        if (runner != null) runner.OnDialogueFinished.RemoveListener(OnDialogueFinished);
    }

    public void Interact()
    {
        if (runner == null) return;

        int questStage = 0;
        if (ContextBootstrapper.ContextSystem.TryGetState(pipesQuestStageKey, out int s)) questStage = s;

        // --- 1. ÅÑËÈ ÒĞÓÁÀ ÏÎ×ÈÍÅÍÀ (ÑÒÀÄÈß 3) ---
        if (questStage >= 3)
        {
            if (myOrderIndex == 3)
            {
                // Äëÿ 3-ãî NPC — äèàëîã 6
                if (fourthNpcAfterRepairGraph != null)
                {
                    _currentGraph = fourthNpcAfterRepairGraph;
                    runner.Play(fourthNpcAfterRepairGraph);
                }
            }
            else
            {
                // Äëÿ 1-ãî è 2-ãî — äèàëîã 7
                if (otherNpcAfterRepairGraph != null)
                {
                    _currentGraph = otherNpcAfterRepairGraph;
                    runner.Play(otherNpcAfterRepairGraph);
                }
            }
            return;
        }

        // --- 2. ÅÑËÈ İÒÎ 3-É NPC È ÊÂÅÑÒ ÒÎËÜÊÎ ÍÀ×ÀËÑß (ÑÒÀÄÈß 1) ---
        if (myOrderIndex == 3 && questStage == 1)
        {
            if (uniqueDialogueGraph != null)
            {
                _currentGraph = uniqueDialogueGraph;
                runner.Play(uniqueDialogueGraph);
                // Ïåğåâîäèì êâåñò íà ñòàäèş 2 (Ñäâèíüòå ÿùåğà è ïî÷èíèòå òğóáó)
                ContextBootstrapper.ContextSystem.TrySetState(pipesQuestStageKey, 2);
                // Îòìå÷àåì, ÷òî ñ ıòèì NPC ïîãîâîğèëè
                ContextBootstrapper.ContextSystem.TrySetState(myUniqueKey, 1);
            }
            return;
        }

        // --- 3. ËÎÃÈÊÀ ÄËß 1-ÃÎ È 2-ÃÎ NPC (È ÏÎÂÒÎĞÍÛÕ ĞÀÇÃÎÂÎĞÎÂ) ---
        bool hasTalked = false;
        if (ContextBootstrapper.ContextSystem.TryGetState(myUniqueKey, out int myState) && myState > 0)
            hasTalked = true;

        if (hasTalked)
        {
            if (fallbackDialogueGraph != null)
            {
                _currentGraph = fallbackDialogueGraph;
                runner.Play(fallbackDialogueGraph);
            }
            return;
        }

        if (myOrderIndex > 1 && previousNpcKey != ContextKey.Dialog_ZombieTalked)
        {
            bool previousTalked = false;
            if (ContextBootstrapper.ContextSystem.TryGetState(previousNpcKey, out int prevState) && prevState > 0)
                previousTalked = true;

            if (!previousTalked)
            {
                if (fallbackDialogueGraph != null)
                {
                    _currentGraph = fallbackDialogueGraph;
                    runner.Play(fallbackDialogueGraph);
                    ContextBootstrapper.ContextSystem.TrySetState(myUniqueKey, 1);
                }
                return;
            }
        }

        if (uniqueDialogueGraph != null)
        {
            _currentGraph = uniqueDialogueGraph;
            runner.Play(uniqueDialogueGraph);
            ContextBootstrapper.ContextSystem.TrySetState(myUniqueKey, 1);
        }
    }

    private void OnDialogueFinished()
    {
        // Ïîñëå 6-ãî äèàëîãà (3-é NPC) ñòàâèì ôèíàëüíóş ñòàäèş 4
        if (myOrderIndex == 3 && _currentGraph == fourthNpcAfterRepairGraph)
        {
            //if (arrowManager != null) arrowManager.ShowArrow();
            ContextBootstrapper.ContextSystem.TrySetState(pipesQuestStageKey, 4);
        }
    }

    public bool CanInteract() => true;
}