using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxLives = 5;
    [SerializeField] float invincibleTime = 1.5f;
    [SerializeField] float flashInterval = 0.1f;
    [SerializeField] SpriteRenderer sprite;

    public UnityEvent<int> onLivesChanged;
    public UnityEvent onGameOver;

    public int Lives { get; private set; }
    public bool IsInvincible { get; private set; }

    void Start()
    {
        Lives = maxLives;
        onLivesChanged?.Invoke(Lives);
    }

    void OnTriggerEnter2D(Collider2D other) => CheckHazard(other);
    void OnTriggerStay2D(Collider2D other)  => CheckHazard(other);

    void CheckHazard(Collider2D other)
    {
        var hazard = other.GetComponentInParent<AttackObject>(); 
        if (hazard == null || IsInvincible || Lives <= 0) return;
        LoseLife();
    }

    public void LoseLife()
    {
        Lives--;
        onLivesChanged?.Invoke(Lives);

        if (Lives <= 0)
        { 
            onGameOver?.Invoke(); 
            return;
        }

        StartCoroutine(Invincibility());
    }

    IEnumerator Invincibility()
    {
        IsInvincible = true;
        float t = 0f;
        
        while (t < invincibleTime)
        {
            sprite.enabled = !sprite.enabled;   // flicker
            yield return new WaitForSeconds(flashInterval);
            t += flashInterval;
        }

        sprite.enabled = true;
        IsInvincible = false;
    }
}