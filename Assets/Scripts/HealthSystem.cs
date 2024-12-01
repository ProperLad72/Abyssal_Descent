using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f; // The maximum health of the boss
    public float currentHealth;    // The current health of the boss

    public void Start()
    {
        // Initialize the boss health
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        // Subtract damage from current health
        currentHealth -= amount;

        // Ensure health doesn't go below 0
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();  // Call the Die method when health is 0
        }
    }

    public void Die()
    {
        // Call your Boss's Die logic here (e.g., spawn exit, destroy boss)
        BossSide bossSide = GetComponent<BossSide>(); // Access BossSide script
        if (bossSide != null)
        {
            bossSide.Die();  // Trigger the exit spawn and other death logic
        }

        Destroy(gameObject); // Destroy the boss object
    }
}