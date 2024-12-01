using System.Collections;

using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 0.5f;       // Range of the melee attack
    public int attackDamage = 1;           // Damage dealt per attack
    public LayerMask enemyLayers;          // Layers that define what counts as an "enemy"
    public Transform attackPoint;          // Point where the attack originates, placed in front of the player

    [Header("Attack Visuals")]
    public GameObject attackVisualizer;    // Visual representation of the attack point (e.g., small circle or sprite)

    private Vector2 lastMovementDirection = Vector2.right; // Default initial direction (right)
    private float attackTimer = 0f; // Timer to control attack cooldown
    public float attackCooldown = 0.5f; // Cooldown duration in seconds
    private bool canAttack = true;  // Flag to check if player can attack

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

        // Check for attack input (Spacebar) and handle cooldown
        if (Input.GetKeyDown(KeyCode.Space) && canAttack)
        {
            Attack();
            StartCoroutine(AttackCooldown());  // Start the cooldown timer after attacking
        }

        // If the attack visual is active, count down the timer
        if (attackVisualizer.activeSelf)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                HideAttackVisualizer();
            }
        }
    }

    void Attack()
    {
        // Visualize or animate the attack if needed
        Debug.Log("Player attacks!");

        // Show the attack visual for a brief moment
        ShowAttackVisualizer();

        // Detect enemies within range of the attack point
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage each enemy hit
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    // Show the attack visual (e.g., a circle or cross)
    void ShowAttackVisualizer()
    {
        attackVisualizer.transform.position = attackPoint.position;  // Position attack visual at the attack point
        attackVisualizer.SetActive(true);  // Activate the attack visualizer
        attackTimer = 0f;  // Reset the attack timer
    }

    // Hide the attack visual after the duration is over
    void HideAttackVisualizer()
    {
        attackVisualizer.SetActive(false);  // Deactivate the attack visualizer
    }

    // Coroutine to handle the cooldown after each attack
    private IEnumerator AttackCooldown()
    {
        canAttack = false;  // Disable attacking while on cooldown
        yield return new WaitForSeconds(attackCooldown);  // Wait for cooldown duration
        canAttack = true;  // Re-enable attacking after cooldown
    }

    // Optional: Draw attack range in the editor for visualization
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
