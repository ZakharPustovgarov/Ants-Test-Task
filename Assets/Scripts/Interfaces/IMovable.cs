using UnityEngine;

internal interface IMovable
{
    public bool MoveTowards(Vector3 destination);
}