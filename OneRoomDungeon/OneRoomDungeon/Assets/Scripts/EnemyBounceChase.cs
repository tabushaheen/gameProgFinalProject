using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBounceChase : MonoBehaviour
{
    [Header("References")]
    public Transform player;                 // drag Player here (or auto-find by tag "Player")
    public Transform pointA;                 // your EnemyPointA
    public Transform pointB;                 // your EnemyPointB

    [Header("Movement")]
    public float moveSpeed = 7f;             // make them faster here
    public float randomSpawnOffsetRadius = 0.6f;
    public bool randomStartAtAorB = true;

    [Header("Wall")]
    public string wallTag = "Wall";
    public float pushOffWallDistance = 0.06f;

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
        // Spawn at A or B (random), with a small random offset so enemies don’t stack
        if (pointA != null && pointB != null)
        {
            Transform spawnPoint = pointA;

            if (randomStartAtAorB)
                spawnPoint = (Random.value < 0.5f) ? pointA : pointB;

            Vector2 off2 = Random.insideUnitCircle * randomSpawnOffsetRadius;
            Vector3 offset = new Vector3(off2.x, 0f, off2.y);

            transform.position = spawnPoint.position + offset;
        }

        // Random initial direction so enemies don't sync
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

        // After wall hit: lock direction toward player's CURRENT position (snapshot)
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

        // Fallback if player missing/overlapping: bounce-ish random or reverse
        if (!hasLockedToPlayerYet)
        {
            // before first lock, randomize to separate enemies even more
            Vector2 r = Random.insideUnitCircle.normalized;
            moveDir = new Vector3(r.x, 0f, r.y);
            if (moveDir.sqrMagnitude < 0.001f) moveDir = -moveDir;
        }
        else
        {
            moveDir = -moveDir;
        }
    }
}
