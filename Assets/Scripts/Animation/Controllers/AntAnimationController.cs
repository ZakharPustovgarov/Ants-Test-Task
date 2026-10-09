using UnityEngine;

public abstract class AntAnimationController : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    private string movingBool = "IsMoving";  
    private string deathTrigger = "Died";
    private string damageTrigger = "TookDamage";

    public void BeginMovement()
    {
        animator.SetBool(movingBool, true);
    }

    public void StopMovement()
    {
        animator.SetBool(movingBool, false);
    }

    public void TakeDamage()
    {
        animator.SetTrigger(damageTrigger);
    }

    public void Die()
    {
        animator.SetTrigger(deathTrigger);
    }
}
