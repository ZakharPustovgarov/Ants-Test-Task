using System;
using UnityEngine;

public abstract class ResourceStorage : MonoBehaviour
{
    [SerializeField] protected int maxCapacity;
    protected int currentCapacity = 0;

    public int MaxCapacity { get { return maxCapacity; } }
    public int CurrentCapacity { get { return currentCapacity; }  }

    public Action<int, int> OnAnthillStorageCurrentCapacityChangedEvent;

    protected virtual int DeliverResource(int count)
    {
        if (currentCapacity + count < maxCapacity)
        {
            currentCapacity += count;
            OnAnthillStorageCurrentCapacityChangedEvent?.Invoke(currentCapacity, maxCapacity);
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
            OnAnthillStorageCurrentCapacityChangedEvent?.Invoke(currentCapacity, maxCapacity);
            return count;
        }

        int buf = currentCapacity;
        OnDepletion();
        return buf;
    }

    protected virtual void OnFull()
    {
        currentCapacity = maxCapacity;
        OnAnthillStorageCurrentCapacityChangedEvent?.Invoke(currentCapacity, maxCapacity);
    }

    protected virtual void OnDepletion()
    {
        currentCapacity = 0;
        OnAnthillStorageCurrentCapacityChangedEvent?.Invoke(currentCapacity, maxCapacity);
    }
}
