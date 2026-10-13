using UnityEngine;

public abstract class ResourceStorage : MonoBehaviour
{
    [SerializeField] protected int maxCapacity;
    protected int currentCapacity = 0;

    public int MaxCapacity { get { return maxCapacity; } }
    public int CurrentCapacity { get { return currentCapacity; }  }


    protected virtual int DeliverResource(int count)
    {
        if (currentCapacity + count < maxCapacity)
        {
            currentCapacity += count;
            OnCapacitryChanged();
            return count;
        }

        OnFull();
        return currentCapacity + count - maxCapacity;
    }

    protected virtual int TakeResource(int count = 1)
    {
        if (currentCapacity - count > 0)
        {
            currentCapacity -= count;
            OnCapacitryChanged();
            return count;
        }

        int buf = currentCapacity;
        OnDepletion();
        return buf;
    }

    protected virtual void OnFull()
    {
        currentCapacity = maxCapacity;
        OnCapacitryChanged();
    }

    protected virtual void OnDepletion()
    {
        currentCapacity = 0;
        OnCapacitryChanged();
    }

    protected virtual void OnCapacitryChanged()
    {

    }
}
