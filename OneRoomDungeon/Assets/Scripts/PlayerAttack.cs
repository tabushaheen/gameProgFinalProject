using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public KeyCode attackKey = KeyCode.Space;
    public float attackDuration = 0.2f;
    public float attackCooldown = 0.3f;

    private Collider hitboxCollider;
    private bool canAttack = true;
    private bool attackActive = false;

    private Transform playerRoot;

    void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        hitboxCollider.enabled = false;

        playerRoot = transform.root;
    }

    void Update()
    {
        if (canAttack && Input.GetKeyDown(attackKey))
        {
            StartCoroutine(DoAttack());
        }
    }

    private IEnumerator DoAttack()
    {
        canAttack = false;
        attackActive = true;
        hitboxCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        hitboxCollider.enabled = false;
        attackActive = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attackActive)
            return;

        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            //one hit per attack per enemy
            enemyHealth.TakeHit(playerRoot.position);
        }
    }
}
