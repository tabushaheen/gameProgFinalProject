using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBounceChase : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform pointA;
    public Transform pointB;

    [Header("Movement")]
    public float moveSpeed = 7f;
    public float randomSpawnOffsetRadius = 0.6f;
    public bool randomStartAtAorB = true;

    [Header("Wall")]
    public string wallTag = "Wall";
    public float pushOffWallDistance = 0.06f;

    [Header("Damage Player On Contact")]
    public int contactDamage = 1;
    public float contactDamageCooldown = 0.75f;

    private float nextDamageTime = 0f;

    private Rigidbody rb;
    private Vector3 moveDir;
    private bool hasLockedToPlayerYet = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Start()
    {
        if (pointA != null && pointB != null)
        {
            Transform spawnPoint = pointA;
            if (randomStartAtAorB)
                spawnPoint = (Random.value < 0.5f) ? pointA : pointB;

            Vector2 off2 = Random.insideUnitCircle * randomSpawnOffsetRadius;
            Vector3 offset = new Vector3(off2.x, 0f, off2.y);
            transform.position = spawnPoint.position + offset;
        }

        Vector2 r = Random.insideUnitCircle.normalized;
        moveDir = new Vector3(r.x, 0f, r.y);
        if (moveDir.sqrMagnitude < 0.001f) moveDir = Vector3.forward;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDir * moveSpeed;
        rb.WakeUp();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Damage player on contact (once per cooldown)
        if (collision.gameObject.CompareTag("Player"))
        {
            TryDamagePlayer(collision.gameObject);
            // don’t return; we still want wall logic when it’s a wall
        }

        if (!collision.gameObject.CompareTag(wallTag))
            return;

        // Push off wall a tiny bit to prevent sticking
        if (collision.contactCount > 0)
        {
            Vector3 n = collision.contacts[0].normal;
            n.y = 0f;
            if (n.sqrMagnitude > 0.0001f)
                transform.position += n.normalized * pushOffWallDistance;
        }

        // Snapshot direction to player on wall hit
        if (player != null)
        {
            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;
            if (toPlayer.sqrMagnitude > 0.001f)
            {
                moveDir = toPlayer.normalized;
                hasLockedToPlayerYet = true;
                return;
            }
        }

        // Fallback
        if (!hasLockedToPlayerYet)
        {
            Vector2 r = Random.insideUnitCircle.normalized;
            moveDir = new Vector3(r.x, 0f, r.y);
            if (moveDir.sqrMagnitude < 0.001f) moveDir = -moveDir;
        }
        else
        {
            moveDir = -moveDir;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Optional: keep damaging while touching (but still cooldown-limited)
        if (collision.gameObject.CompareTag("Player"))
            TryDamagePlayer(collision.gameObject);
    }

    private void TryDamagePlayer(GameObject playerObj)
    {
        if (Time.time < nextDamageTime) return;

        PlayerHealth ph = playerObj.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(contactDamage);
            nextDamageTime = Time.time + contactDamageCooldown;
        }
    }
}
