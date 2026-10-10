using UnityEngine;

public class AnthillStorage : ResourceStorage, IInsertable, IExtractable
{
    void Start()
    {
        currentCapacity = 0;
    }

    public int Extract(int count = 1)
    {
        return base.TakeResource(count);
    }

    public void Insert(int count)
    {
        base.DeliverResource(count);
    }
}
