using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHP = 100;
    public float currentHP;
    public EnemyBrain brain;
    public Animator anim;
    private bool isAlive = true;
    public ParticleSystem particles;
    public enemHealthBar healthBar;

    private void Start()
    {
        currentHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
        particles.Stop(); // Ensure the particle system is initially stopped
        isAlive = true;
    }

    public void TakeDamage(float damageAmount)
    {
        if (!isAlive)
        {
            return; // if enemy dead, do nothing
        }

        currentHP -= damageAmount;

        if (currentHP <= 0)
        {
            Die(); // Enemy has no health left, initiate death
        }
        else
        {
            anim.SetTrigger("hit"); // Play a hit animation
            healthBar.SetHealth(currentHP); // Update the health bar
        }

        if (currentHP < 40 && currentHP >= 1)
        {
            particles.Play(); // Play particle effects for low health
            StartCoroutine(RegenerateHealth()); // Initiate health regeneration
        }
    }

    private void Die()
    {
        isAlive = false; // Mark the enemy as dead
        Destroy(brain); // Remove the enemy's brain or AI control
        currentHP = 0; 
        particles.Stop(); // Stop the particle system
        anim.SetFloat("Speed", 0f); 
        anim.SetTrigger("die"); // Play death animation
        healthBar.SetHealth(0); // Update the health bar to zero
    }

    private IEnumerator RegenerateHealth()
    {
        while (currentHP < maxHP)
        {
            currentHP += Time.deltaTime;

            // Check if the enemy is still alive before updating the health bar
            if (isAlive)
            {
                healthBar.SetHealth(currentHP); // Update the health bar during regeneration
            }

            yield return null;
        }

        currentHP = maxHP; // Ensure health is exactly maxHP
        particles.Stop(); // Stop particle effects after regeneration
    }
}
