using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;

    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    private Transform currentTarget;

    private void Start()
    {
        //if points are missing, just stop
        if (pointA == null || pointB == null)
        {
            Debug.LogWarning("EnemyPatrol: pointA or pointB not assigned on " + gameObject.name);
            enabled = false;
            return;
        }

        // Start at point A and move towards B
        transform.position = pointA.position;
        currentTarget = pointB;
    }

    private void Update()
    {
        if (pointA == null || pointB == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            moveSpeed * Time.deltaTime
        );

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        if (distanceToTarget < 0.1f)
        {
            if (currentTarget == pointA)
            {
                currentTarget = pointB;
            }
            else
            {
                currentTarget = pointA;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
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