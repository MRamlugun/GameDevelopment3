using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private Movement move; // Reference to the Movement component for controlling player movement

    public Animator anim;
    public bool isTakingDamage = false; // Flag to check if the player is taking damage
    private float lerpTimer;
    public float chipSpeed = 2f;

    public Image FrontHealthBar; // The front health bar UI element
    public Image BackHealthBar; // The back health bar UI element

    void Awake()
    {
        currentHealth = maxHealth; // Initialize the player's health to its maximum value
    }

    void Update()
    {
        UpdateHealthUI(); // Update the player's health bar UI
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth); // Reduce the player's health and ensure it stays within the valid range
        lerpTimer = 0f;

        damage = Mathf.Clamp(damage, 0, int.MaxValue); // Ensure damage is a valid value

        if (currentHealth <= 0)
        {
            Die(); // Player has run out of health, initiate death
        }
        else {
            anim.SetTrigger("hit"); // Play a hit animation
            isTakingDamage = true;
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = FrontHealthBar.fillAmount; // Get the fill amount of the front health bar
        float fillB = BackHealthBar.fillAmount; // Get the fill amount of the back health bar
        float hFraction = currentHealth / maxHealth; // Calculate the fraction of current health over max health

        if (fillB > hFraction)
        {
            // Update health bars based on the health fraction
            FrontHealthBar.fillAmount = hFraction; // Set the front health bar fill amount
            BackHealthBar.color = Color.red; // Change color to indicate health decrease
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            BackHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete); // Smoothly interpolate back health bar
        }
    }

    private void Die()
    {
        anim.SetTrigger("die"); // Play the death animation
        Destroy(move); // Remove the player's ability to move
    }
}
