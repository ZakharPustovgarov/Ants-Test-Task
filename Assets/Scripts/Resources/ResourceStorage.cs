using UnityEngine;

public abstract class ResourceStorage : MonoBehaviour
{
    [SerializeField] protected int maxCapacity;
    protected int currentCapacity;

    protected virtual void DeliverResource(int count)
    {
        if (currentCapacity + count < maxCapacity)
        {
            currentCapacity += count;
            return;
        }

        OnFull();
    }

    protected virtual int TakeResource(int count = 1)
    {
        if (currentCapacity - count > 0)
        {
            currentCapacity -= count;
            return count;
        }

        int buf = currentCapacity;
        OnDepletion();
        return buf;
    }

    protected virtual void OnFull()
    {
        currentCapacity = maxCapacity;
    }

    protected virtual void OnDepletion()
    {
        currentCapacity = 0;
    }
}
