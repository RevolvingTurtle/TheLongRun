using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump")]
    public float jumpVelocity = 12f;
    public float coyoteTime = 0.08f;
    public float jumpBuffer = 0.10f;

    [Header("Duck")]
    public KeyCode duckKey = KeyCode.DownArrow;
    public KeyCode duckKeyAlt = KeyCode.S;
    public float duckHeightMultiplier = 0.6f;
    public bool duckOnlyOnGround = true;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundSkin = 0.05f;

    [Header("Hat")]
    public Transform hatTransform;

    public Vector3 standingHatLocalPosition = new Vector3(0f, 0.6f, 0f);
    public Vector3 standingHatLocalScale = Vector3.one;

    public Vector3 duckingHatLocalPosition = new Vector3(0.15f, 0.3f, 0f);
    public Vector3 duckingHatLocalScale = new Vector3(0.8f, 0.8f, 1f);

    Rigidbody2D rb;
    BoxCollider2D box;

    float coyoteTimer;
    float bufferTimer;

    Vector2 standingSize;
    Vector2 standingOffset;

    Vector2 duckSize;
    Vector2 duckOffset;

    bool isDucking;
    AudioSource audioSource;
    public GameObject explosionPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        standingSize = box.size;
        standingOffset = box.offset;

        duckSize = new Vector2(standingSize.x, standingSize.y * duckHeightMultiplier);

        float standingBottom = standingOffset.y - standingSize.y * 0.5f;
        duckOffset = new Vector2(standingOffset.x, standingBottom + duckSize.y * 0.5f);

        if (hatTransform != null)
        {
            hatTransform.localPosition = standingHatLocalPosition;
            hatTransform.localScale = standingHatLocalScale;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!GameManager.I.isRunning) return;

        Bounds b = box.bounds;
        Vector2 checkSize = new Vector2(b.size.x * 0.9f, groundSkin);
        Vector2 checkPos = new Vector2(b.center.x, b.min.y - groundSkin * 0.5f);
        bool grounded = Physics2D.OverlapBox(checkPos, checkSize, 0f, groundLayer);

        if (grounded) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        bool duckHeld = Input.GetKey(duckKey) || Input.GetKey(duckKeyAlt);

        if (duckOnlyOnGround)
        {
            if (grounded && duckHeld) StartDuck();
            else StopDuck();
        }
        else
        {
            if (duckHeld) StartDuck();
            else StopDuck();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0))
            bufferTimer = jumpBuffer;
        else
            bufferTimer -= Time.deltaTime;

        if (!isDucking && bufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    if (audioSource != null)
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
    }

            bufferTimer = 0f;
            coyoteTimer = 0f;
        }
    }

    void StartDuck()
    {
        if (isDucking) return;
        isDucking = true;

        box.size = duckSize;
        box.offset = duckOffset;

        transform.localScale = new Vector3(1f, 0.7f, 1f);

        if (hatTransform != null)
        {
            hatTransform.localPosition = duckingHatLocalPosition;
            hatTransform.localScale = duckingHatLocalScale;
        }
    }

    void StopDuck()
    {
        if (!isDucking) return;
        isDucking = false;

        box.size = standingSize;
        box.offset = standingOffset;

        transform.localScale = Vector3.one;

        if (hatTransform != null)
        {
            hatTransform.localPosition = standingHatLocalPosition;
            hatTransform.localScale = standingHatLocalScale;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Vector3 hitPosition = col.contacts[0].point;

            Die(hitPosition);

            Destroy(col.gameObject);

            GameManager.I.GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Vector3 hitPosition = transform.position;

            Die(hitPosition);
            GameManager.I.GameOver();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!box) box = GetComponent<BoxCollider2D>();
        if (!box) return;

        Bounds b = box.bounds;
        Vector2 checkSize = new Vector2(b.size.x * 0.9f, groundSkin);
        Vector2 checkPos = new Vector2(b.center.x, b.min.y - groundSkin * 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(checkPos, checkSize);
    }
    public void Die(Vector3 hitPosition)
    {
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = false;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, hitPosition, Quaternion.identity);
    }
}