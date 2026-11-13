using UnityEngine;

public class ContextBootstrapper : MonoBehaviour
{
    public static ContextSystem ContextSystem { get; private set; }

    [SerializeField] private ContextDefaults _defaults;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic() => ContextSystem = null;

    private void Awake()
    {
        if (ContextSystem != null)
        {
            Destroy(gameObject);
            return;
        }

        if (_defaults == null)
        {
            Debug.LogError(
                "ContextBootstrapper: не назначен ContextDefaults. " +
                "Контексты не будут объявлены.", this);
            return;
        }

        ContextSystem = new ContextSystem();
        _defaults.DeclareAll(ContextSystem);

        DontDestroyOnLoad(gameObject);
    }
}