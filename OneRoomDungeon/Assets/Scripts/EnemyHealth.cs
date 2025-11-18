using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 2;
    private int currentHealth;

    [Header("Knockback")]
    public float knockbackForce = 10f;

    private Rigidbody rb;

    void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    public void TakeHit(Vector3 hitSourcePosition)
    {
        Debug.Log($"{name} took a hit! Current health BEFORE hit: {currentHealth}");

        currentHealth--;

        if (rb != null)
        {
            Vector3 knockDirection = (transform.position - hitSourcePosition).normalized;
            knockDirection.y = 0f;
            rb.AddForce(knockDirection * knockbackForce, ForceMode.Impulse);
        }
        
        // Check if player died
        if (currentHealth <= 0)
        {
            Debug.Log($"{name} died.");
            Destroy(gameObject);
        }
    }
}
