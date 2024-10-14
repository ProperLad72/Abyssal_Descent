using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;
    public float health = 0f;
    [SerializeField] public float maxHealth = 100f;

    private void Start(){
        health = maxHealth;
        UpdateHealthBar();
    }

    public void UpdatedHealth(float mod){
        health += mod;

        if (health > maxHealth){
            health = maxHealth;
        }
        else if (health <= 0f){
            health = 0f;
            Debug.Log("Player Respawn");
        }
        UpdateHealthBar();
    }
    // Function to update the health bar based on the current health
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Set the fill amount of the health bar based on current health as a percentage of max health
            healthBar.fillAmount = health / maxHealth;

            // Optionally, change the color based on health percentage (e.g., red when low)
            if (health / maxHealth > 0.5f)
            {
                healthBar.color = Color.green;  // Healthy color
            }
            else if (health / maxHealth > 0.2f)
            {
                healthBar.color = Color.yellow; // Caution color
            }
            else
            {
                healthBar.color = Color.red;    // Critical color
            }

            // Ensure the health bar is visible
            healthBar.gameObject.SetActive(true);
        }
    }
}