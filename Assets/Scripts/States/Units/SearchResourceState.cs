using System;
using UnityEngine;

public class SearchResourceState : UnitState
{
    private ResourceVein desirableVein;
    private float desirableWayChance;
    private LoaderAnt loaderAnt;
    private Vector3 movePosition;

    public Action<ResourceVein> OnResourceFind;

    public SearchResourceState(ResourceVein desirableVein, float desirableWayChance = 0.7f)
    {
        this.desirableVein = desirableVein;
        this.desirableWayChance = desirableWayChance;
    }

    public override void Enter(Creature unit)
    {
        base.Enter(unit);

        if (unit is LoaderAnt)
            loaderAnt = unit as LoaderAnt;
        else
            throw new ArgumentException("Unit isn't LoaderAnt in SearchResourceState");

        loaderAnt.OnObjectDetected += OnDetection;
        AssignMovePosition();
        StartMovement();
    }

    public override void Execute()
    {
        if(loaderAnt.MoveTowards(movePosition))
        {
            AssignMovePosition();
        }
    }

    public override void Exit()
    {
        loaderAnt.OnObjectDetected -= OnDetection;
        StopMovement();
    }

    private void AssignMovePosition()
    {
        if(UnityEngine.Random.value <= desirableWayChance)
        {
            movePosition = desirableVein.transform.position;
            return;
        }

        movePosition = GetRandomPoint();
    }

    public Vector2 GetRandomPoint()
    {
        Vector2 currentPos = loaderAnt.transform.position;
        Vector2 direction = new Vector2(desirableVein.transform.position.x, desirableVein.transform.position.y) - currentPos;
        float distance = direction.magnitude;

        float randomAngle = UnityEngine.Random.Range(10, 350);

        Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);

        Vector2 rotatedDirection = rotation * direction.normalized * distance;

        return currentPos + rotatedDirection;
    }

    private void StartMovement()
    {
        loaderAnt.StartMovementAnimation();
    }

    private void StopMovement()
    {
        loaderAnt.StopMovementAnimation();
    }

    private void OnDetection(GameObject obj)
    {
        ResourceVein vein = obj.GetComponent<ResourceVein>();

        if (vein == null) return;

        OnResourceFind?.Invoke(vein);
    }
}
