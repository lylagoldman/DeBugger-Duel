using UnityEngine;

public class FlipStateBehaviour : StateMachineBehaviour
{
    private SpriteRenderer spriteRenderer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        animator.SetBool("IsFacingLeft", !spriteRenderer.flipX);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Turn")) 
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
