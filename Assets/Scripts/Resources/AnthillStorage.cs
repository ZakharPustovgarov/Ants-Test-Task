using SimpleEventBus;

public class AnthillStorage : ResourceStorage, IInsertable, IExtractable
{
    void Start()
    {
        currentCapacity = 0;
        OnCapacitryChanged();
    }

    public int Extract(int count = 1)
    {
        return base.TakeResource(count);
    }

    public int Insert(int count)
    {
        return base.DeliverResource(count);
    }

    protected override void OnCapacitryChanged()
    {
        base.OnCapacitryChanged();
        EventBus.Instance.Publish<AnthillStorageCapacityChanged>(new AnthillStorageCapacityChanged(currentCapacity, maxCapacity));
    }
}
