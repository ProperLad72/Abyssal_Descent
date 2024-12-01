using UnityEngine;

public class BossSide : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public float patrolSpeed = 2f; // Speed for side-to-side movement
    public float chaseSpeed = 4f; // Speed when chasing the player
    public float patrolDistance = 5f; // Distance to patrol side to side
    public float aggroRange = 7f; // Distance to trigger chase mode

    private Vector3 startPoint; // Starting position for patrol
    private bool movingRight = true; // Direction of patrol
    private bool isChasing = false; // Whether the boss is chasing the player

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

    private void OnDrawGizmosSelected()
    {
        // Draw aggro range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}