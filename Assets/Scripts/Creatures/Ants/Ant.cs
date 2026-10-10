using System.Collections;
using UnityEngine;

public abstract class Ant : MovableCreature
{
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

    protected abstract void StartDeathAnimation();

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
