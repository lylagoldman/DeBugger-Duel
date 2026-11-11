using System;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float speed = 8.0f;
    private bool hasCollided = false;

    private Animator bulletAnimator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform bulletTransform;
   
    private float directionMultiplier;

    void Start()
    {
        bulletAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        bulletTransform = GetComponent<Transform>();

        if (spriteRenderer.flipX)
        {
            directionMultiplier = -1.0f;
        } else
        {
            directionMultiplier = 1.0f;
        }
    }

    void FixedUpdate()
    { 
        if (!hasCollided)
        {
            if (Math.Abs(bulletTransform.localPosition.x) > 16.0f)
            {
                EndOfLife();
            } else
            {
                rb.linearVelocityX = speed * directionMultiplier;
            }
        } else {
            rb.linearVelocityX = 0;
        }
    }
        
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            bulletAnimator.SetTrigger("Impact");
            hasCollided = true;
        }
    }

    public void EndOfLife()
    {
        Destroy(gameObject);
    }
}

