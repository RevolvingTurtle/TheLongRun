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
    public float groundSkin = 0.05f; // thin strip below collider

    Rigidbody2D rb;
    BoxCollider2D box;

    float coyoteTimer;
    float bufferTimer;

    Vector2 standingSize;
    Vector2 standingOffset;

    Vector2 duckSize;
    Vector2 duckOffset;

    bool isDucking;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        standingSize = box.size;
        standingOffset = box.offset;

        duckSize = new Vector2(standingSize.x, standingSize.y * duckHeightMultiplier);

        float standingBottom = standingOffset.y - standingSize.y * 0.5f;
        duckOffset = new Vector2(standingOffset.x, standingBottom + duckSize.y * 0.5f);
    }

    void Update()
    {
        if (!GameManager.I.isRunning) return;

        Bounds b = box.bounds;
        Vector2 checkSize = new Vector2(b.size.x * 0.9f, groundSkin);
        Vector2 checkPos = new Vector2(b.center.x, b.min.y - groundSkin * 0.5f);
        bool grounded = Physics2D.OverlapBox(checkPos, checkSize, 0f, groundLayer);

        // coyote time
        if (grounded) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        // duck input
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

        // jump buffer
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0))
            bufferTimer = jumpBuffer;
        else
            bufferTimer -= Time.deltaTime;

        // jump (block while ducking)
        if (!isDucking && bufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
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
    }

    void StopDuck()
    {
        if (!isDucking) return;
        isDucking = false;

        box.size = standingSize;
        box.offset = standingOffset;

        transform.localScale = Vector3.one;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Die();
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

    public void Die()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}