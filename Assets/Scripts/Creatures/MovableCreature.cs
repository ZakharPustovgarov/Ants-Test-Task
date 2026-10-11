using UnityEngine;

public abstract class MovableCreature : Creature, IMovable
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 1;

    public bool MoveTowards(Vector3 destination, float stoppingDistance = 0)
    {
        Vector3 directionToTarget = destination - transform.position;
        float currentDistance = directionToTarget.magnitude;

        if(stoppingDistance == 0)
        {
            float distanceSqr = directionToTarget.sqrMagnitude;

            if (distanceSqr < 0.0001f)
            {

                transform.position = destination;
                return true;
            }
        }
        else
        {
            if (currentDistance <= stoppingDistance)
            {
                return true;
            }
        }

        Vector3 moveDirection = directionToTarget.normalized;

        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle);
        }

        Vector3 nextStep = transform.position + moveDirection * speed * Time.deltaTime;

        rb.MovePosition(nextStep);

        return false;
    }
}
