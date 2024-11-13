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

    private void Update()
    {
        // Move toward the player if in range
        if (target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if the attack timer has reached the attack speed threshold
            if (canAttack >= attackSpeed)
            {
                // Get the PlayerHealth component from the player and deal damage
                PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.UpdatedHealth(-attackDamage);  // Apply damage to the player
                }

                // Reset the attack timer
                canAttack = 0f;
            }
            else
            {
                // Increment the attack timer by the time that has passed since the last frame
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

    // Method to take damage from player's attack
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took " + damage + " damage. Health remaining: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    // Method to destroy the enemy when health reaches zero
    private void Die()
    {
        Debug.Log("Enemy died!");
        // Add death animation or sound effects here if needed
        Destroy(gameObject); // Remove enemy from scene
    }
}
