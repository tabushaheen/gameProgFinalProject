using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Patrol")]
    [Tooltip("Points the enemy will move between. For now, set size = 2 (A and B).")]
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    public float arriveThreshold = 0.1f;

    private int currentPointIndex = 0;

    [Header("Health")]
    public int maxHealth = 2;      // 2 hits to kill
    private int currentHealth;

    [Header("Knockback")]
    public float knockbackForce = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            // Start at first point
            transform.position = patrolPoints[0].position;
            currentPointIndex = 1 % patrolPoints.Length;
        }
    }

    void Update()
    {
        HandlePatrol();
    }

    void HandlePatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPointIndex];

        // Move toward current patrol point
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // Check if we arrived close enough
        float distance = Vector3.Distance(transform.position, targetPoint.position);
        if (distance <= arriveThreshold)
        {
            // Go to next point (loops: 0 -> 1 -> 2 -> ... -> 0)
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    public void TakeDamage(int damageAmount, Vector3 hitFromPosition)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage. Current HP: " + currentHealth);

        ApplyKnockback(hitFromPosition);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ApplyKnockback(Vector3 hitFromPosition)
    {
        if (rb == null)
            return;

        // Direction from attacker to enemy
        Vector3 knockDirection = (transform.position - hitFromPosition).normalized;
        knockDirection.y = 0f; // keep it flat (top-down)

        rb.velocity = Vector3.zero; // reset current velocity
        rb.AddForce(knockDirection * knockbackForce, ForceMode.Impulse);
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}