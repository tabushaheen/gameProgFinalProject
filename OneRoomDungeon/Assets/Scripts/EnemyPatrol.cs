using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;

    [Header("Patrol Points (at least 2)")]
    public Transform[] patrolPoints;

    private int currentIndex = 0;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void Start()
    {
        if (patrolPoints == null || patrolPoints.Length < 2)
        {
            Debug.LogWarning("EnemyPatrol: Need at least 2 patrol points on " + gameObject.name);
            enabled = false;
            return;
        }

        //start at the first point
        transform.position = patrolPoints[0].position;

        currentIndex = 1;
    }

    private void Update()
    {
        if (patrolPoints == null || patrolPoints.Length < 2) return;

        Transform targetPoint = patrolPoints[currentIndex];

        Vector3 newPos = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        rb.MovePosition(newPos);

        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);

        if (distanceToTarget < 0.1f)
        {
            IncrementPatrolIndex();
        }
    }

    private void IncrementPatrolIndex()
    {
        currentIndex++;

        if (currentIndex >= patrolPoints.Length)
        {
            currentIndex = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if hitting wall, switch patrol target
        if (collision.gameObject.CompareTag("Wall"))
        {
            IncrementPatrolIndex();
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }
}