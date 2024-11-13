using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float directionChangeInterval = 2f;
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    
    private float canAttack;
    private Vector2 moveDirection;
    private float directionChangeTimer;

    private void Start()
    {
        ChooseNewDirection();
    }

    private void Update()
    {
        // Update the position of the enemy based on the chosen direction
        float step = speed * Time.deltaTime;
        transform.position += (Vector3)moveDirection * step;

        // Update the timer and change direction if needed
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer >= directionChangeInterval)
        {
            ChooseNewDirection();
            directionChangeTimer = 0f;
        }
    }

    private void ChooseNewDirection()
    {
        // Choose a random direction for the enemy to move in
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
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
}
