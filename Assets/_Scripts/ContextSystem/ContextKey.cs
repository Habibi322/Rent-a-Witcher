using UnityEngine;

public enum ContextKey
{
    // Диалоги (поговорили с зомби)
    Dialog_ZombieTalked = 0,
    Dialog_NewBiomeTalked = 20,
    // Квесты (стадии)
    Quest_ZombieFenceStarted = 1,
    Quest_NewBiomeTaskStarted = 21,

    // Предметы (в инвентаре)
    Item_Boards = 10,
    Item_NewBiomeItem = 22,

        // Новый биом: 4 NPC
    Quest_NewBiomeTalked_1 = 30,
    Quest_NewBiomeTalked_2 = 31,
    Quest_NewBiomeTalked_3 = 32,

    // Стадии квеста с трубами
    Quest_PipesStage = 40
}
