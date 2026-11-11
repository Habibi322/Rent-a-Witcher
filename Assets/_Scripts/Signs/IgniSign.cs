using UnityEngine;

public class IgniSign : Sign
{
    [Header("Настройки Игни")]
    [SerializeField] private float fireDuration = 2f;
    [SerializeField] private ParticleSystem igniParticles;

    private void Awake()
    {
        signName = "Igni";
        cooldown = 1f;
    }

    public void ActivateOnTarget(GameObject target)
    {
        if (Time.time - lastUsedTime < cooldown)
        {
            return;
        }

        lastUsedTime = Time.time;

        IIgnitable ignitable = target.GetComponent<IIgnitable>();
        if (ignitable != null)
        {
            bool success = ignitable.Ignite(fireDuration);

            if (success)
            {

                if (igniParticles != null)
                    igniParticles.Play();

                PlayVisualEffect();
            }
        }
    }

    public override bool CanInteractWith(GameObject target)
    {
        return target.GetComponent<IIgnitable>() != null;
    }
}