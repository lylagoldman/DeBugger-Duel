using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{   
    public GameObject bullet;
    public GameObject fireball;

    [SerializeField] float fireRate = 0.08f;
    [SerializeField] Transform standardFirePoint;
    [SerializeField] Transform duckFirePoint;

    private Animator playerAnimator;
    private float nextFireTime = 0f;
    private bool isValidAttack;
    private bool isHoldingShoot;


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        isValidAttack = playerAnimator.GetBool("IsDucking") == false;
    }

    void Update()
    {
        // Shoot standard bullets, recalculate time allowed before next bullet
        if (isHoldingShoot && isValidAttack && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bullet, standardFirePoint.position, standardFirePoint.rotation);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // Duck Attack, spawn fireball
        if (context.performed && playerAnimator.GetBool("IsDucking") && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("DuckIdle"))
        {
            playerAnimator.SetTrigger("DuckAttack");
        }
        
        // Sets isHoldingShoot for only standard shooting, not Duck Attack
        if (context.performed && !playerAnimator.GetBool("IsDucking"))
        {
            isHoldingShoot = true;  
        } else if (context.canceled && !playerAnimator.GetBool("IsDucking"))
        {
            isHoldingShoot = false;
        }
    }

    public void SpawnFireball()
    {
        Instantiate(fireball, duckFirePoint.position, duckFirePoint.rotation);
    }
}
