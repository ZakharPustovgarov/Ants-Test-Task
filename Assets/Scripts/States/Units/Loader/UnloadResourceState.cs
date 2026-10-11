
using System;

public class UnloadResourceState : UnitState
{
    private IInsertable storage;
    private LoaderAnt loaderAnt;

    public Action<Creature> OnResourceUnloadedEvent;

    public UnloadResourceState(IInsertable storage)
    {
        this.storage = storage;
    }

    public override void Enter(Creature unit)
    {
        base.Enter(unit);

        loaderAnt = ClassUtility.CastOrThrow<LoaderAnt>(unit, "SearchResourceState");

        loaderAnt.OnResourceUnloadedEvent += OnResourceUnloaded;
        
    }

    public override void Execute()
    {
        loaderAnt.Unload(storage);
    }

    public override void Exit()
    {
        loaderAnt.OnResourceUnloadedEvent -= OnResourceUnloaded;
    }

    private void OnResourceUnloaded()
    {
        OnResourceUnloadedEvent?.Invoke(loaderAnt);
    }
}
