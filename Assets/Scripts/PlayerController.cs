using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -45f;

    private Rigidbody2D rb;
    private Animator animator;
    
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isFacingLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();;
        isFacingLeft = animator.GetBool("IsFacingLeft");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * Physics2D.gravity.y * rb.gravityScale));
            isGrounded = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ValidateMove();
        CheckForDuck();
        CheckForTurn();
    }

    public void ValidateMove()
    {
        if (Math.Abs(moveInput.y) < 0.35 && Math.Abs(moveInput.x) > 0.1)
        {
            rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
        } else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        animator.SetFloat("SpeedX", Math.Abs(rb.linearVelocityX));
        animator.SetBool("IsRunning", rb.linearVelocityX != 0); 
        animator.SetBool("IsGrounded", isGrounded);
    }

    public void CheckForDuck()
    {
        if ((moveInput.y < -0.65) && isGrounded && (rb.linearVelocityX == 0) && (animator.GetBool("IsDucking") == false))
        {
            animator.SetTrigger("Duck");
            animator.SetBool("IsDucking", true);
        } else if ((moveInput.y >= -0.35) && (animator.GetBool("IsDucking") == true))
        {
            animator.SetBool("IsDucking", false);
        }
    }

    public void CheckForTurn()
    {
        isFacingLeft = animator.GetBool("IsFacingLeft");

        if (((!isFacingLeft && moveInput.x < -0.1) || (isFacingLeft && moveInput.x > 0.1)) && animator.GetBool("IsDucking") == false)
        {
            animator.SetTrigger("Turn");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
