using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;
    public Animator animator;
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (animator.GetBool("HasCollided") == false)
        {
            rb.linearVelocityX = speed;
        } else
        {
            rb.linearVelocityX = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetBool("HasCollided", true);
        }
    }
}

