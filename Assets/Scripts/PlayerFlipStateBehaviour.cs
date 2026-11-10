using UnityEngine;

public class PlayerFlipStateBehaviour : StateMachineBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform standardFirePoint;
    private Transform duckFirePoint;

    void Awake()
    {
        standardFirePoint = GameObject.Find("StandardFirePoint").GetComponent<Transform>();
        duckFirePoint = GameObject.Find("DuckFirePoint").GetComponent<Transform>();
    }

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
            FlipFirePoints();
        }
    }

    public void FlipFirePoints()
    {
        Vector3 standardPos = standardFirePoint.localPosition;
        standardPos.x *= -1;            
        standardFirePoint.localPosition = standardPos;

        Vector3 duckPos = duckFirePoint.localPosition;
        duckPos.x *= -1;
        duckFirePoint.localPosition = duckPos;
    }
}
