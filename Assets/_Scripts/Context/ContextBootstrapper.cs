using UnityEngine;

public class ContextBootstrapper : MonoBehaviour
{
    public static ContextSystem ContextSystem { get; private set; }

    private void Awake()
    {
        if (ContextSystem != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        ContextSystem = new ContextSystem();

        // ќбъ€вл€ем все ключи и их начальные значени€ (0)
        ContextSystem.DeclareContext(ContextKey.Dialog_ZombieTalked, 0);
        ContextSystem.DeclareContext(ContextKey.Quest_ZombieFenceStarted, 0);
        ContextSystem.DeclareContext(ContextKey.Quest_ZombieFenceCompleted, 0);
        ContextSystem.DeclareContext(ContextKey.Item_Boards, 0);
        ContextSystem.DeclareContext(ContextKey.Item_Nails, 0);
        ContextSystem.DeclareContext(ContextKey.Item_Hammer, 0);
    }
}