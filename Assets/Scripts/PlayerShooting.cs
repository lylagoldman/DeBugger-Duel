using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{   
    public GameObject bullet;
    public GameObject fireball;

    [SerializeField] float fireRate = 0.08f;
    [SerializeField] Transform standardFirePoint;
    [SerializeField] Transform duckFirePoint;
    
    public bool CanFireball => playerAnimator.GetBool("IsDucking") && SpecialMeter.Instance != null && SpecialMeter.Instance.HasCharge;

    private Animator playerAnimator;
    private float nextFireTime = 0f;
    private bool isHoldingShoot;


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Shoot standard bullets, recalculate time allowed before next bullet
        if (isHoldingShoot && !playerAnimator.GetBool("IsDucking") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bullet, standardFirePoint.position, standardFirePoint.rotation);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // Duck Attack, spawn fireball
        if (CanFireball && context.performed && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("DuckIdle"))
        {
            SpecialMeter.Instance.TrySpend();
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
