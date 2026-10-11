
using System;

public class UnloadResourceState : UnitState
{
    private IInsertable storage;
    private LoaderAnt loaderAnt;

    public Action OnResourceUnloadedEvent;

    public UnloadResourceState(IInsertable storage)
    {
        this.storage = storage;
    }

    public override void Enter(Creature unit)
    {
        base.Enter(unit);

        loaderAnt = ClassUtility.CastOrThrow<LoaderAnt>(unit, "SearchResourceState");

        loaderAnt.OnResourceUnloadedEvent += OnResourceUnloaded;
        loaderAnt.Unload(storage);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        loaderAnt.OnResourceUnloadedEvent -= OnResourceUnloaded;
    }

    private void OnResourceUnloaded()
    {
        OnResourceUnloadedEvent?.Invoke();
    }
}
