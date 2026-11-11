using UnityEngine;

public abstract class Sign : MonoBehaviour
{
    [Header("Настройки знака")]
    [SerializeField] protected string signName;
    [SerializeField] protected float cooldown = 1f;

    protected float lastUsedTime = 0f;

    public string SignName => signName;

    public virtual void Activate()
    {
        if (Time.time - lastUsedTime < cooldown)
        {
            return;
        }

        lastUsedTime = Time.time;

        OnSignActivated();
    }

    protected virtual void OnSignActivated()
    {
    }

    public abstract bool CanInteractWith(GameObject target);

    protected virtual void PlayVisualEffect()
    {
    }
}