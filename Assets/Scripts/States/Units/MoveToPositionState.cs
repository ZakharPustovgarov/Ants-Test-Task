
using System;
using UnityEngine;

public class MoveToPositionState : UnitState
{
    public Action OnTargetReached;
    private Vector3 destination;
    private float stoppingDistance;
    private IMovable movableUnit;

    public MoveToPositionState(Vector3 pos, float stoppingDistance = 0)
    {
        this.stoppingDistance = stoppingDistance;
    }

    public override void Enter(Creature unit)
    {
        base.Enter(unit);

        movableUnit = ClassUtility.CastOrThrow<IMovable>(unit, "SearchResourceState");
    }

    public override void Execute()
    {
        if (movableUnit.MoveTowards(destination, stoppingDistance))
            OnTargetReached?.Invoke();
    }

    public override void Exit()
    {

    }
}
