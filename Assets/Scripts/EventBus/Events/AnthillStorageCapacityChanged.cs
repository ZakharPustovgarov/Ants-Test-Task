using SimpleEventBus;

public class AnthillStorageCapacityChanged : IEvent
{
    public int CurrentCapacity { get; }
    public int MaxCapacity { get; }

    public AnthillStorageCapacityChanged(int current, int max)
    {
        CurrentCapacity = current;
        MaxCapacity = max;
    }
}
