using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -45f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 velocity;
    private bool facingRight = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {moveInput}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"Jumping: {context.performed} - Is Grounded: {controller.isGrounded}");
        if (context.performed && controller.isGrounded)
        {
            Debug.Log("We are supposed to jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void flipAcrossX()
    {
        facingRight = !facingRight;

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (facingRight && moveInput.x < 0)
        {
            flipAcrossX();
        } else if (!facingRight && moveInput.x > 0)
        {
            flipAcrossX();
        }

        Vector2 move = new Vector2(moveInput.x, 0);
        controller.Move(move * speed * Time.deltaTime);
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);   
    }
}
