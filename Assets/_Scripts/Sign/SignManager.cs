using UnityEngine;
using System.Collections.Generic;

public class SignManager : MonoBehaviour
{
    [SerializeField] private List<Sign> signs = new List<Sign>();
    private List<GameObject> objectsInRange = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!objectsInRange.Contains(other.gameObject))
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInRange.Remove(other.gameObject);
    }

    public void TryUseSign()
    {

        foreach (GameObject target in objectsInRange)
        {
            foreach (Sign sign in signs)
            {
                if (sign.CanInteractWith(target))
                {
                    if (sign is AardSign aardSign)
                    {
                        aardSign.ActivateAard(target);
                    }
                    else if (sign is IgniSign igniSign)
                    {
                        igniSign.ActivateOnTarget(target);
                    }
                    else
                    {
                        sign.Activate();
                    }

                    return;
                }
            }
        }
    }
}