using UnityEngine;

public class LoaderAnimationController : AntAnimationController
{
    private string attackTrigger = "Attack";
    private string actingBool = "IsActing";

    public void BeginActing()
    {
        animator.SetBool(actingBool, true);
    }

    public void StopActing()
    {
        animator.SetBool(actingBool, false);
    }

    public void Attack()
    {
        animator.SetTrigger(attackTrigger);
    }
}
