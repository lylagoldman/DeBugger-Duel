using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float speed = 8.0f;
    private SpriteRenderer player;
    private Animator bulletAnimator;
    private Rigidbody2D rb;
    private float directionMultiplier = 1.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        bulletAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        CheckFacingDirection();
    }

    // Update is called once per frame
    void Update()
    { 
        if (!bulletAnimator.GetBool("HasCollided"))
        {
            rb.linearVelocityX = speed * directionMultiplier;
        } else {
            rb.linearVelocityX = 0;
        }
    }

    private void CheckFacingDirection()
    {
        if (player.flipX)
        {
            directionMultiplier = -1.0f;
        } else
        {
            directionMultiplier = 1.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            bulletAnimator.SetBool("HasCollided", true);
        }
    }
}

