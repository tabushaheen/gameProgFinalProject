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

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Start()
    {
        // Spawn near A or B
        if (pointA != null && pointB != null)
        {
            Transform spawnPoint = pointA;
            if (randomStartAtAorB)
                spawnPoint = (Random.value < 0.5f) ? pointA : pointB;

            Vector2 off2 = Random.insideUnitCircle * randomSpawnOffsetRadius;
            Vector3 offset = new Vector3(off2.x, 0f, off2.y);

            rb.position = spawnPoint.position + offset;
        }

        Vector2 r = Random.insideUnitCircle.normalized;
        moveDir = new Vector3(r.x, 0f, r.y);
        if (moveDir.sqrMagnitude < 0.001f) moveDir = Vector3.forward;
    }

    void FixedUpdate()
    {
        Vector3 newPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Always resolve to root so child colliders don't break logic
        Transform otherRoot = collision.transform.root;

        // Damage player on contact (once per cooldown)
        if (otherRoot.CompareTag("Player"))
        {
            TryDamagePlayer(otherRoot.gameObject);
        }

        // Wall bounce logic (NO teleporting!)
        if (!collision.gameObject.CompareTag(wallTag))
            return;

        // Reflect direction using the wall normal (stable bounce)
        if (collision.contactCount > 0)
        {
            Vector3 n = collision.contacts[0].normal;
            n.y = 0f;

            if (n.sqrMagnitude > 0.0001f)
                moveDir = Vector3.Reflect(moveDir, n.normalized).normalized;
        }

        // After bouncing, optionally "lock" toward player so the enemy starts chasing
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

        // Fallback direction logic
        if (!hasLockedToPlayerYet)
        {
            Vector2 r = Random.insideUnitCircle.normalized;
            moveDir = new Vector3(r.x, 0f, r.y);
            if (moveDir.sqrMagnitude < 0.001f) moveDir = Vector3.forward;
        }
        else
        {
            moveDir = -moveDir;
        }
    }

    /*
    private void OnCollisionStay(Collision collision)
    {
        Transform otherRoot = collision.transform.root;
        if (otherRoot.CompareTag("Player"))
            TryDamagePlayer(otherRoot.gameObject);
    }
    */

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
