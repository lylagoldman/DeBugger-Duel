using System;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public float horizontalSpeed = 10.0f;
    public float initialVerticalSpeed = 15.0f;
    [SerializeField] float curveSteepness = 2f;
    
    private float elapsedTime = 0f;
    private bool hasCollided = false;

    private Animator fireballAnimator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform fireballTransform;

    private float directionMultiplier;

    void Start()
    {
        fireballAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        fireballTransform = GetComponent<Transform>();

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
            elapsedTime += Time.fixedDeltaTime;
            
            if (Math.Abs(fireballTransform.localPosition.x) > 16.0f)
            {
                EndOfLife();
            }
            
            float blendFactor = Mathf.Exp(-curveSteepness * elapsedTime);
            float vx = horizontalSpeed * (1f - blendFactor) * directionMultiplier;
            float vy = initialVerticalSpeed * blendFactor;
            rb.linearVelocity = new Vector2(vx, vy);
            
            if (rb.linearVelocity.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(vy, vx) * Mathf.Rad2Deg;
                rb.MoveRotation(angle);
            }
        } else
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
        
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            fireballAnimator.SetTrigger("Impact");
            hasCollided = true;
        }
    }

    public void EndOfLife()
    {
        Destroy(gameObject);
    }
}