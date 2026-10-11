using System;
using UnityEngine;

public class MoveToPositionState : UnitState
{
    private Vector3 destination;
    private float stoppingDistance;
    private IMovable movableUnit;

    public Action<IMovable> OnTargetReachedEvent;

    public MoveToPositionState(Vector3 pos, float stoppingDistance = 0)
    {
        this.stoppingDistance = stoppingDistance;
        destination = pos;
    }

    public override void Enter(Creature unit)
    {
        base.Enter(unit);

        movableUnit = ClassUtility.CastOrThrow<IMovable>(unit, "SearchResourceState");
    }

    public override void Execute()
    {
        if (movableUnit.MoveTowards(destination, stoppingDistance))
            OnTargetReachedEvent?.Invoke(movableUnit);
    }

    public override void Exit()
    {

    }
}
