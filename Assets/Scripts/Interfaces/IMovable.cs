using UnityEngine;

public interface IMovable
{
    public bool MoveTowards(Vector3 destination, float stoppingDistance);
}