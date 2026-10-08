using UnityEngine;

public class AnthillStorage : ResourceStorage, IInsertable, IExtractable
{
    void Start()
    {
        currentCapacity = 0;
    }

    public void Extract(int count = 1)
    {
        base.TakeResource(count);
    }

    public void Insert(int count)
    {
        base.DeliverResource(count);
    }
}
