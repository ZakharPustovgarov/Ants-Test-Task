using System;

public abstract class UnitState
{
    protected Creature assignedUnit;

    public virtual void Enter(Creature unit)
    {
        assignedUnit = unit;
    }

    public abstract void Execute();
    public abstract void Exit();
}
