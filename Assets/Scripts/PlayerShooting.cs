using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{   
    public GameObject bullet;

    [SerializeField] float fireRate = 0.8f;
    [SerializeField] Transform standardFirePoint;
    [SerializeField] Transform duckFirePoint;

    private Animator animator;
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
        // Shoot standard bullet
        if (isHoldingShoot && isValidAttack && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bullet, standardFirePoint.position, standardFirePoint.rotation);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // Duck Attack trigger
        if (context.performed && (animator.GetBool("IsDucking") == true))
        {
            animator.SetTrigger("DuckAttack");
            //fireballAnimator.SetTrigger("Shoot");
        }
        
        // Sets isHoldingShoot for only standard shooting, not Duck Attack
        if (context.performed && (animator.GetBool("IsDucking") == false))
        {
            isHoldingShoot = true;  
        } else if (context.canceled && (animator.GetBool("IsDucking") == false))
        {
            isHoldingShoot = false;
        }
    }
}
