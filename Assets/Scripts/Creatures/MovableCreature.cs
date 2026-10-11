using UnityEngine;

public abstract class MovableCreature : Creature, IMovable
{
    [SerializeField] private float speed = 1;

    public bool MoveTowards(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        Vector3 direction = (destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }

        float distanceSqr = (destination - transform.position).sqrMagnitude;

        if (distanceSqr < 0.0001f) // 0.0001f Ч это 0.01 в квадрате
        {
            
            transform.position = destination;
            return true; 
        }

        return false;
    }
}
