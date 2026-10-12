using SimpleEventBus;

public class ResourceVeinSpawned : IEvent
{
    public ResourceVein Vein { get; }

    public ResourceVeinSpawned(ResourceVein vein)
    {
        Vein = vein;
    }
}
