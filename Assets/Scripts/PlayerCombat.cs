using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 0.5f;       // Range of the melee attack
    public int attackDamage = 1;           // Damage dealt per attack
    public LayerMask enemyLayers;          // Layers that define what counts as an "enemy"
    public Transform attackPoint;          // Point where the attack originates, placed in front of the player

    private Vector2 lastMovementDirection = Vector2.right; // Default initial direction (right)

    void Update()
    {
        // Track the player's movement direction for attack orientation
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movementInput != Vector2.zero)
        {
            lastMovementDirection = movementInput.normalized;
        }

        // Update the attackPoint position based on the player's movement direction
        if (attackPoint != null)
        {
            attackPoint.localPosition = lastMovementDirection * attackRange;
        }

        // Check for attack input (Spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Visualize or animate the attack if needed
        Debug.Log("Player attacks!");

        // Detect enemies within range of the attack point
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage each enemy hit
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    // Optional: Draw attack range in the editor for visualization
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
