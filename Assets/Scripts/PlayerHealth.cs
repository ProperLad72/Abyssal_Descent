using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;  // Reference to the health bar fill image
    public float health = 0f;
    [SerializeField] public float maxHealth = 100f;

    private void Start()
    {
        health = maxHealth;  // Initialize health to the maximum value
        UpdateHealthBar();   // Ensure the health bar is updated at the start
    }

    public void UpdatedHealth(float mod)
    {
        // Update health based on the modifier (positive or negative)
        health += mod;

        // Clamp health between 0 and maxHealth to avoid overflow or underflow
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0f)
        {
            health = 0f;
            Debug.Log("Player Respawn");  // Implement respawn logic here if needed
        }

        UpdateHealthBar();   // Update the health bar after the health change
    }

    // Function to update the health bar based on the player's current health
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Calculate the fill amount based on current health as a percentage of max health
            float fillAmount = health / maxHealth;

            // Ensure fillAmount is between 0 and 1 (for the Image component)
            fillAmount = Mathf.Clamp(fillAmount, 0f, 1f);

            // Set the fill amount to adjust the health bar size
            healthBar.fillAmount = fillAmount;

            // Optionally, change the health bar color based on the percentage of health
            if (fillAmount > 0.5f)
            {
                healthBar.color = new Color(0f, 0.58f, 0f);// Healthy color
            }
            else if (fillAmount > 0.2f)
            {
                healthBar.color = Color.yellow; // Caution color
            }
            else
            {
                healthBar.color = Color.red;    // Critical health color
            }
        }
    }
}
