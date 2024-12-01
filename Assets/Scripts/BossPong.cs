using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPong : MonoBehaviour
{
    [Header("Boss Settings")]
    public float moveSpeed = 5f; // Speed of the boss
    public Vector2 initialDirection = new Vector2(1, 1); // Initial direction of movement
    public int health = 10; // Boss health
    public GameObject holeToExit; // The exit object to enable upon boss defeat

    private Rigidbody2D rb; // Rigidbody for movement
    private Vector2 currentDirection; // Current movement direction

    private void Awake()
    {
        // Initialize Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Configure Rigidbody
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Set initial movement direction
        currentDirection = initialDirection.normalized;

        // Ensure the hole is inactive at start
        if (holeToExit != null)
        {
            holeToExit.SetActive(false);
        }
        else
        {
            Debug.LogWarning("HoleToExit is not assigned. Assign it in the Inspector.");
        }
    }

    private void FixedUpdate()
    {
        // Move the boss in the current direction
        rb.velocity = currentDirection * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reverse direction when hitting walls
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Get the normal of the wall collision
            Vector2 normal = collision.contacts[0].normal;

            // Check if the boss is hitting a corner (or sharp angle)
            // If it's a sharp angle, we limit the reflection change.
            if (Mathf.Abs(Vector2.Dot(currentDirection, normal)) > 0.5f)
            {
                // A sharp angle hit (like a corner) - reverse just the X or Y component
                if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
                {
                    currentDirection.x = -currentDirection.x; // Reverse the horizontal direction
                }
                else
                {
                    currentDirection.y = -currentDirection.y; // Reverse the vertical direction
                }
            }
            else
            {
                // Regular reflection
                currentDirection = Vector2.Reflect(currentDirection, normal).normalized;
            }
        }

        // Damage the player if collided, but don't change movement direction
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.UpdatedHealth(-50); // Damage the player
                Debug.Log("Boss collided with Player. Player takes damage.");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduce boss health
        health -= damage;
        Debug.Log($"Boss takes {damage} damage! Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss has been defeated!");

        // Activate the exit hole
        if (holeToExit != null)
        {
            holeToExit.SetActive(true);
            Debug.Log("Exit hole activated!");
        }
        else
        {
            Debug.LogError("HoleToExit is not assigned.");
        }

        // Destroy the boss object
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the initial movement direction in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)initialDirection.normalized);
    }
}
