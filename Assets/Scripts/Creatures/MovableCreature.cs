using UnityEngine;

public abstract class MovableCreature : Creature, IMovable
{
    [SerializeField] private float speed = 1;

    public void MoveTowards(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        Vector3 direction = (destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }
}
