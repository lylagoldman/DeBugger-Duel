using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{   
    public GameObject bullet;

    [SerializeField] float fireRate = 0.8f;
    [SerializeField] Transform firePoint;
    
    private Animator animator;
    [SerializeField] Animator bulletAnimator;
    [SerializeField] Animator fireballAnimator;
  
    private float nextFireTime = 0f;
    private bool isValidAttack;
    private bool isHoldingShoot;


    void Start()
    {
        animator = GetComponent<Animator>();
        isValidAttack = animator.GetBool("IsDucking") == false;
    }

    void Update()
    {
        if (isHoldingShoot && isValidAttack && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && (animator.GetBool("IsDucking") == true))
        {
            animator.SetTrigger("DuckAttack");
            //fireballAnimator.SetTrigger("Shoot");
        }
        
        if (context.performed && (animator.GetBool("IsDucking") == false))
        {
            isHoldingShoot = true;  
        } else if (context.canceled && (animator.GetBool("IsDucking") == false))
        {
            isHoldingShoot = false;
        }
    }
}
