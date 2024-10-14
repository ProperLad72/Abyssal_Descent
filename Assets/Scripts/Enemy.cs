using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;

    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackSpeed = 1f;
    private float canAttack;
    private Transform target;

    private void Update()
    {
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
        if (other.gameObject.tag == "Player")
        {
            target = other.transform;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = null;

        }
    }
}
