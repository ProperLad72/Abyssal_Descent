using UnityEngine;

public class BossSide : MonoBehaviour
{
    [Header("Boss Settings")]
    public Transform player; // Reference to the player's Transform
    public float patrolSpeed = 2f; // Speed for side-to-side movement
    public float chaseSpeed = 4f; // Speed when chasing the player
    public float patrolDistance = 5f; // Distance to patrol side to side
    public float aggroRange = 7f; // Distance to trigger chase mode
    public float attackDamage = 20f; // Damage dealt to the player
    public float attackCooldown = 1f; // Cooldown time between attacks

    public int health = 100; // Boss's initial health
    public Transform exitSpawnPoint; // Location to spawn the exit
    public GameObject exitPrefab; // The exit prefab to instantiate

    private Vector3 startPoint; // Starting position for patrol
    private bool movingRight = true; // Direction of patrol
    private bool isChasing = false; // Whether the boss is chasing the player
    private float nextAttackTime = 0f; // Timer to control attack cooldown

    private void Start()
    {
        // Set the starting point for patrol
        startPoint = transform.position;

        // Automatically find the player if not assigned in the Inspector
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player GameObject not found. Make sure the Player has the 'Player' tag.");
            }
        }
    }

    private void Update()
    {
        if (player == null) return; // Ensure player reference exists

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= aggroRange)
        {
            // Start chasing the player
            isChasing = true;
        }
        else
        {
            // Return to patrolling if the player is out of range
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // Move side to side
        float patrolLimit = patrolDistance / 2f;
        Vector3 targetPosition = startPoint + (movingRight ? Vector3.right : Vector3.left) * patrolLimit;

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        // If reached the patrol limit, switch direction
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            movingRight = !movingRight;
        }
    }

    private void ChasePlayer()
    {
        // Move towards the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null && Time.time >= nextAttackTime)
            {
                playerHealth.UpdatedHealth(-attackDamage); // Deal damage to the player
                Debug.Log("Boss hit player! Player health: " + playerHealth.health);
                nextAttackTime = Time.time + attackCooldown; // Set the cooldown
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Boss takes {damage} damage! Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Boss has died!");

        // Spawn the exit at the designated spawn point
        if (exitSpawnPoint != null && exitPrefab != null)
        {
            Instantiate(exitPrefab, exitSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("ExitPrefab or ExitSpawnPoint is not assigned!");
        }

        Destroy(gameObject); // Destroy the boss
    }

    private void OnDrawGizmosSelected()
    {
        // Draw aggro range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        // Draw the exit spawn point for debugging
        if (exitSpawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(exitSpawnPoint.position, 0.2f);
        }
    }
}
