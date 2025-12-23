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

    [Header("Death")]
    public float destroyDelay = 1.2f; // match your death clip length

    private Rigidbody rb;
    private Animator anim;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    public void TakeHit(Vector3 hitSourcePosition)
    {
        if (isDead) return;

        Debug.Log($"{name} took a hit! Current health BEFORE hit: {currentHealth}");
        currentHealth--;

        if (rb != null)
        {
            Vector3 knockDirection = (transform.position - hitSourcePosition).normalized;
            knockDirection.y = 0f;
            rb.AddForce(knockDirection * knockbackForce, ForceMode.Impulse);
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log($"{name} died.");

            if (anim != null)
                anim.SetTrigger("Die");

            Destroy(gameObject, destroyDelay);
        }
    }
}
