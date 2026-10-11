using System;

public class GatherResourceState : UnitState
{
    private ResourceVein vein;
    private LoaderAnt loaderAnt;

    public Action<Creature> OnResourceGatheredEvent;

    public GatherResourceState(ResourceVein vein)
    {
        this.vein = vein;
    }

    public override void Enter(Creature unit)
    {
        base.Enter(unit);

        loaderAnt = ClassUtility.CastOrThrow<LoaderAnt>(unit, "SearchResourceState");

        loaderAnt.OnResourceGatheredEvent += OnResourceGathered;
        loaderAnt.Gather(vein);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        loaderAnt.OnResourceGatheredEvent -= OnResourceGathered;
    }

    private void OnResourceGathered()
    {
        OnResourceGatheredEvent?.Invoke(loaderAnt);
    }
}
