using UnityEngine;
using UnityEngine.U2D.Animation;

public class KeyScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SpriteLibrary library;
    [SerializeField] SpriteResolver resolver;
    [SerializeField] BoxCollider2D box;

    [Header("Launch")]
    [SerializeField] float horizontalSpeed = 7f;
    [SerializeField] float launchUpSpeed = 5f;
    [SerializeField] float gravity = 25f;

    [Header("Bouncing")]
    [SerializeField] float groundY = -3f;
    [SerializeField] float keyHalfHeight = 0.3f;
    [SerializeField, Range(0f, 1f)] float bounciness = 0.6f;
    [SerializeField] float horizontalLossPerBounce = 0.9f;

    [Header("End of life")]

    [SerializeField] float breakDuration = 0.3f;
    [SerializeField] float killX = -20f;
    [SerializeField] int breakOnBounce = 3;
    
    int bounceCount;
    float vx, vy;
    bool breaking;
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (library == null) library = GetComponent<SpriteLibrary>();
        if (resolver == null) resolver = GetComponent<SpriteResolver>();
        if (box == null) box = GetComponent<BoxCollider2D>();
        vx = -horizontalSpeed;   // defaults; Launch() can override these
        vy = launchUpSpeed;
    }

    public void Setup(KeyType type, FxColor color)
    {
        if (type == null) { Debug.LogWarning($"{name}: Setup got a null KeyType (empty slot in keyTypes array?)"); return; }

        bool red = color == FxColor.Red;
        SpriteLibraryAsset asset = red ? type.red : type.purple;

        if (asset == null)
        {
            Debug.LogWarning($"{type.name} has no {(red ? "red" : "purple")} library assigned, using purple");
            asset = type.purple;
            red = false;
        }
        if (asset == null) { Debug.LogError($"{type.name} has no libraries at all"); return; }

        library.spriteLibraryAsset = asset;
        resolver.SetCategoryAndLabel("Tumble", "0");
        resolver.ResolveSpriteToSpriteRenderer();

        if (box != null) box.size = type.colliderSize;
        bounciness = type.bounciness;
    }

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

        vy -= gravity * dt;
        pos.y += vy * dt;
        pos.x += vx * dt;

        if (pos.y <= restY)
        {
            pos.y = restY;
            bounceCount++;

            if (bounceCount >= breakOnBounce)
            {
                transform.position = pos;
                Break();
                return;
            }

            vy = -vy * bounciness;
            vx *= horizontalLossPerBounce;
        }

        transform.position = pos;
    }

    void Break()
    {
        breaking = true;
        if (box != null) box.enabled = false;
        if (anim != null) anim.SetTrigger("break");
        Destroy(gameObject, breakDuration);
    }
}