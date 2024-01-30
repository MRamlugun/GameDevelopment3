using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagicSystem : MonoBehaviour
{
    [SerializeField] private Spell spellToCast;
    
    // Maximum mana the enemy can have
    [SerializeField] public float maxMana = 100f;
    
    // Current mana the enemy has
    [SerializeField] public float currentMana;
    
    // Rate at which mana recharges
    [SerializeField] private float manaRechargeRate = 5.0f;
    
    // Time to wait for mana to recharge when it's below the maximum
    [SerializeField] private float timetoWaitForRecharge = 1f;
    
    // Timer to keep track of mana recharge
    private float currentManaRechargeTimer;
    
    // Time between casting spells
    [SerializeField] private float timeBetweenCasts = 0.25f;
    
    // Timer to keep track of the time since the last spell was cast
    private float currentCastTimer;

    public Animator anim;

    [HideInInspector]
    public bool castingMagic = false;

    // Reference to the point from which spells are cast
    [SerializeField] private Transform castPoint;

    
    private void Awake()
    {
        // Initialize the animator reference
        anim = GetComponent<Animator>();

        // Start with the maximum amount of mana
        currentMana = maxMana;
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the enemy has enough mana to cast the spell
        bool hasEnoughMana = currentMana - spellToCast.spellToCast.ManaCost >= 0f;
        
        // Check if the enemy can start casting a spell
        if (!castingMagic && anim.GetBool("Spell") && hasEnoughMana)
        {
            // Set the casting flag to true
            castingMagic = true;
            
            // Deduct the mana cost of the spell
            currentMana -= spellToCast.spellToCast.ManaCost;
            
            // Reset timers for casting and mana recharge
            currentCastTimer = 0;
            currentManaRechargeTimer = 0;
            
            castSpell();
        }

        // Check if the enemy is currently casting a spell
        if (castingMagic)
        {
            currentCastTimer += Time.deltaTime;

            // If the casting time has exceeded the cooldown, stop casting
            if (currentCastTimer > timeBetweenCasts) 
                castingMagic = false;
        }

        // Check if the enemy's mana is below the maximum and it's not currently casting or using the spell
        if (currentMana < maxMana && !castingMagic && !(anim.GetBool("Spell")) )
        {
            currentManaRechargeTimer += Time.deltaTime;

            // If the recharge time has passed, start recharging mana
            if (currentManaRechargeTimer > timetoWaitForRecharge)
            {
                currentMana += manaRechargeRate * Time.deltaTime;
            
                // Ensure that the current mana doesn't exceed the maximum
                if (currentMana > maxMana) 
                    currentMana = maxMana;
            }
        } 
    }

    public bool HasEnoughMana()
    {
        return currentMana >= maxMana;
    }

    // Method to cast the spell
    void castSpell()
    {
        // Cast the spell by instantiating the spell at the cast point
        Instantiate(spellToCast, castPoint.position, castPoint.rotation);
    }

    

}
