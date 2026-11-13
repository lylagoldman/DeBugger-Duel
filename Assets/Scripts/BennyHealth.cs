using UnityEngine;
using UnityEngine.Events;

public class BennyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 1500;
    [SerializeField] int phaseTwoAt = 750;
    [SerializeField] SpriteRenderer sprite;
    public UnityEvent onDefeat;

    static readonly int HealthHash = Animator.StringToHash("Health");
    static readonly int PhaseHash = Animator.StringToHash("NextPhase");
    static readonly int DefeatHash = Animator.StringToHash("Defeat");

    Animator anim;
    int health;
    bool phaseTwo, defeated;

    void Awake()
    {
        anim = GetComponent<Animator>();
        health = maxHealth;
        anim.SetInteger(HealthHash, health);
    }

    public void TakeDamage(int amount)
    {
        if (defeated) return;

        health = Mathf.Max(0, health - amount);
        Debug.Log("Benny Health:" + health);
        anim.SetInteger(HealthHash, health);
        StartCoroutine(HitFlash());

        if (health == 0)
        {
            defeated = true;
            anim.SetTrigger(DefeatHash);
            onDefeat?.Invoke();
        }
        else if (!phaseTwo && health <= phaseTwoAt)
        {
            phaseTwo = true;
            anim.SetTrigger(PhaseHash);
        }
    }

    System.Collections.IEnumerator HitFlash()
    {
        sprite.color = new Color(1f, 0.6f, 0.6f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = Color.white;
    }
}