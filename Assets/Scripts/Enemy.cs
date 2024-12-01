using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;

    [Header("Combat Settings")]
    public int health = 3; // Enemy health
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackSpeed = 1f; // Time between attacks
    private float attackCooldown = 0f; // Timer for attack frequency
    private Transform target; // Player target

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>(); // Add Rigidbody2D if missing
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Configure Rigidbody2D
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0; // Disable gravity
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            // Calculate direction toward the target
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.velocity = direction * speed; // Set velocity toward the target
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving if no target
        }
    }

    private void Update()
    {
        // Update attack cooldown
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null && attackCooldown <= 0f)
            {
                playerHealth.UpdatedHealth(-attackDamage); // Deal damage to the player
                Debug.Log($"Enemy attacks! Player Health: {playerHealth.health}");

                attackCooldown = attackSpeed; // Reset attack cooldown
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform; // Set player as the target
            Debug.Log("Player detected. Enemy starts chasing.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = null; // Stop chasing the player
            Debug.Log("Player exited range. Enemy stops chasing.");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; // Reduce enemy health
        Debug.Log($"Enemy takes {damage} damage! Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy dies!");
        Destroy(gameObject); // Destroy the enemy object
    }
}