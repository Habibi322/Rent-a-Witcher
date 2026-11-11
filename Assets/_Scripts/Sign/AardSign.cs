using UnityEngine;

public class AardSign : Sign
{
    [Header("Настройки Аарда")]
    [SerializeField] private ParticleSystem aardParticles;
    [SerializeField] private AardManager aardManager;

    private void Awake()
    {
        signName = "Аард";
        cooldown = 1f;
    }

    public void ActivateAard(GameObject target)
    {
        if (Time.time - lastUsedTime < cooldown)
        {
            return;
        }

        lastUsedTime = Time.time;

        if (target.GetComponent<Rigidbody>() == null)
        {
            return;
        }

        if (aardManager != null)
            aardManager.ActivateAard(target);

        if (aardParticles != null)
            aardParticles.Play();

        PlayVisualEffect();
    }

    public override bool CanInteractWith(GameObject target)
    {
        return target.GetComponent<Rigidbody>() != null;
    }

}