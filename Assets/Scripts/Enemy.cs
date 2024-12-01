using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;

    [Header("Combat Settings")]
    public int health = 3;                // Enemy health
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackSpeed = 1f;
    private float canAttack;
    private Transform target;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody2D>(); // Add Rigidbody2D if missing
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Configure Rigidbody2D
        rb.gravityScale = 0; // No gravity for 2D movement
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation

        // Set sorting layer for correct rendering
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 2; // Ensure enemies render above floors
        }
    }

    private void FixedUpdate()
    {
        // Move toward the player if a target exists
        if (target != null)
        {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canAttack >= attackSpeed)
            {
                PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.UpdatedHealth(-attackDamage);
                }
                canAttack = 0f;
            }
            else
            {
                canAttack += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
