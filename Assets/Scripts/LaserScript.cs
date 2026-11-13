using System.Collections;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] BoxCollider2D hitbox;
    Animator anim;
    float sustainTime = 1.2f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        hitbox.enabled = false;
    }

    public void Init(float sustain) => sustainTime = sustain;

    public void HitboxOn()  => hitbox.enabled = true;
    public void HitboxOff() => hitbox.enabled = false;

    public void BeginSustain() => StartCoroutine(SustainRoutine());

    IEnumerator SustainRoutine()
    {
        yield return new WaitForSeconds(sustainTime);
        anim.SetTrigger("dissipate");
    }

    public void Finished() => Destroy(gameObject);

    public void EndOfLife()
    {
        Destroy(gameObject);
    }
}