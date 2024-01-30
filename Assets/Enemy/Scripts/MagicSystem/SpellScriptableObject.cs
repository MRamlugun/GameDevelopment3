using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellScriptableObject", menuName = "spells")]
public class SpellScriptableObject : ScriptableObject
{
    public float ManaCost = 5f;
    public float LifeTime = 2f;
    public float speed = 15f;
    public float damageAmount = 15f;
    public float SpellRadius = 0.5f;

    private float cooldown = 0f; // Cooldown time in seconds
    private bool isOnCooldown = false;
    private float lastCastTime = 0f; // Time when the spell was last cast

    // Callback for updating the cooldown
    public void UpdateCooldown()
    {
        if (isOnCooldown && Time.time - lastCastTime >= cooldown)
        {
            isOnCooldown = false;
        }
    }

    // Check if the spell can be cast
    public bool CanCastSpell()
    {
        return !isOnCooldown;
    }

    // Use this method to cast the spell
    public void CastSpell()
    {
        if (!isOnCooldown)
        {
            // Put your spell casting logic here

            // Set the cooldown with a random duration
            cooldown = Random.Range(5f, 10f); // Adjust the range as needed
            isOnCooldown = true;

            // Record the time the spell was cast
            lastCastTime = Time.time;
        }
    }
}
