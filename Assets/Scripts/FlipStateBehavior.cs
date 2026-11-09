using UnityEngine;

public class FlipStateBehaviour : StateMachineBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform firePoint;

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
            
            firePoint = GameObject.Find("StandardFirePoint").GetComponent<Transform>();
            Vector3 scale = firePoint.localScale;
            scale.x *= -1;
            firePoint.localScale = scale;
        }
    }
}
