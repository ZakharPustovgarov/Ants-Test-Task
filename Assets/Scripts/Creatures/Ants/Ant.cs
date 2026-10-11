using System.Collections;
using UnityEngine;

public abstract class Ant : MovableCreature
{
    [SerializeField] protected AntAnimationController _animController;
    [SerializeField] private float deathTimer = 3f;
    protected UnitState currentState;

    protected virtual void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(UnitState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    public void StartMovementAnimation()
    {
        _animController.StartMovement();
    }

    public void StopMovementAnimation()
    {
        _animController.StopMovement();
    }

    private void StartDeathAnimation()
    {
        _animController.Die();
    }

    protected IEnumerator WaitAndDie(float delay)
    {
        yield return new WaitForSeconds(delay);

        this.gameObject.SetActive(false);
    }

    protected override void OnDeath()
    {
        StartDeathAnimation();
        StartCoroutine(WaitAndDie(deathTimer));
    }
}
