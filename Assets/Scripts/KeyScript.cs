using UnityEngine.U2D.Animation;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [Header("Launch")]
    [SerializeField] float horizontalSpeed = 7f;
    [SerializeField] float launchUpSpeed = 5f;
    [SerializeField] float gravity = 25f;

    [Header("Bouncing")]
    [SerializeField] float groundY = -1.1f;            // world Y of the walking surface
    [SerializeField] float keyHalfHeight = 0.2f;     // so the key rests ON the ground, not in it
    [SerializeField, Range(0f, 1f)] float bounciness = 0.6f;   // each bounce keeps 60% of its speed
    [SerializeField] float stopBounceSpeed = 2f;
    [SerializeField] float horizontalLossPerBounce = 0.9f;

    [Header("End of life")]
    [SerializeField] float slideFriction = 6f;
    [SerializeField] float breakAfterSettled = 0.6f;
    [SerializeField] float killX = -18f;
    float vx, vy;
    bool settled, breaking;
    float settledTimer;
    Animator anim;

    [SerializeField] SpriteLibrary library;
    [SerializeField] SpriteResolver resolver;
    [SerializeField] BoxCollider2D box;

    void Awake() => anim = GetComponent<Animator>();

    public void Setup(KeyType type, FxColor color)
    {
        bool red = color == FxColor.Red;
        library.spriteLibraryAsset = red ? type.red : type.purple;   // green falls back to purple
        resolver.ResolveSpriteToSpriteRenderer();

        box.size = type.colliderSize;
        bounciness = type.bounciness;
        gameObject.tag = red ? "Parryable" : "Hazard";
    }

    void Start()
    {
        vx = -horizontalSpeed;
        vy = launchUpSpeed;
    }

    // Optional: lets BennyAttacks vary each key's launch
    public void Launch(float speed, float upSpeed)
    {
        vx = -speed;
        vy = upSpeed;
    }

    void Update()
    {
        if (breaking) return;
        KeyMovement();
        if (transform.position.x < killX) Destroy(gameObject);
    }

    void KeyMovement()
    {
        float dt = Time.deltaTime;
        Vector3 pos = transform.position;
        float restY = groundY + keyHalfHeight;

        if (!settled)
        {
            vy -= gravity * dt;
            pos.y += vy * dt;
            pos.x += vx * dt;

            if (pos.y <= restY)                       // hit the ground
            {
                pos.y = restY;
                vy = -vy * bounciness;                // flip and shrink the bounce
                vx *= horizontalLossPerBounce;

                if (vy < stopBounceSpeed)             // too small to bounce again
                {
                    vy = 0f;
                    settled = true;
                }
            }
        }
        else
        {
            pos.y = restY;
            vx = Mathf.MoveTowards(vx, 0f, slideFriction * dt);   // skid to a stop
            pos.x += vx * dt;

            settledTimer += dt;
            if (settledTimer >= breakAfterSettled) Break();
        }

        transform.position = pos;
    }

    void Break()
    {
        breaking = true;
        if (anim != null) anim.SetTrigger("break");
        Destroy(gameObject, 0.3f);                    // roughly the length of your break clip
    }
}