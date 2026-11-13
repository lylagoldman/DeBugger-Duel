using UnityEngine;

public class RainDrop : MonoBehaviour
{
    [SerializeField] float fallSpeed = 8f;
    [SerializeField] float groundY = -3f;
    [SerializeField] float halfHeight = 0.3f;
    [SerializeField] float breakDuration = 0.4f;
    [SerializeField] Collider2D hitbox;

    Animator anim;
    bool broken;

    void Awake() => anim = GetComponent<Animator>();

    void Update()
    {
        if (broken) return;

        Vector3 pos = transform.position;
        pos.y -= fallSpeed * Time.deltaTime;

        if (pos.y <= groundY + halfHeight)
        {
            pos.y = groundY + halfHeight;
            broken = true;
            hitbox.enabled = false;
            anim.SetTrigger("Break");
            Destroy(gameObject, breakDuration);
        }
        transform.position = pos;
    }
}