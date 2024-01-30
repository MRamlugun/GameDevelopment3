using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public Transform target;
    
    private EnemyReferences enemyRef;
    private EnemyHealth health;
    private EnemyMagicSystem magic;
    private WeaponManager bullet;
    private PlayerStats stats;

    #region Movement and behavior settings
    public float spawnDistance = 5.0f;
    private float pathUpdateDeadline;
    private float fightingDistance;
    private bool isAlive = true;

    #endregion

    // Attacking
    private bool alreadyAttacked;
    private float damage = 5f;
    private float spellDamage = 10f;

    private bool isCastingSpell = false;
    private float spellCooldown = 5.0f;
    private float lastSpellCastTime;

    public EnemySpawner enemySpawner;
    private bool hasSpawnedEnemies = false;


    private void Awake()
    {
        // Initialize references in Awake method
        enemyRef = GetComponent<EnemyReferences>();
        magic = GetComponent<EnemyMagicSystem>();
        health = GetComponent<EnemyHealth>();
        stats = target.GetComponent<PlayerStats>();
    }


    void Start()
    {
        // Initialize starting values and references
        fightingDistance = enemyRef.agent.stoppingDistance;
        enemySpawner = GetComponent<EnemySpawner>();
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        if (target != null)
        {
            if (stats.currentHealth <= 0)
            {
                isAlive = false;
                enemyRef.anim.SetTrigger("won");
                DestroySpawnedEnemies();
            }
            // Check if the player is in the attack range or not
            bool inRange = Vector3.Distance(transform.position, target.position) <= fightingDistance;

            if (inRange)
            {
                LookAtTarget();
            }
            else
            {
                UpdatePath();
            }

            // Set the 'attack' animation parameter based on the range
            enemyRef.anim.SetBool("attack", inRange);

            if (isCastingSpell)
            {
                // Check if it's time to stop casting the spell
                if (Time.time - lastSpellCastTime > spellCooldown)
                {
                    isCastingSpell = false;
                    enemyRef.anim.SetBool("Spell", false); // Set the spell animation to false
                }
            }
            else
            {
                CheckAttack();
            }

            // Check if time to start casting a spell
            if (!isCastingSpell && isAlive)
            {
                if (!inRange && magic.HasEnoughMana())
                {
                    TryCastSpell();
                }
            }

            if (health.currentHP <= 40 && !hasSpawnedEnemies)
            {
                // Activate the EnemySpawner script when health drops below 40
                enemySpawner.StartCoroutine(enemySpawner.EnemySpawn(transform));
                hasSpawnedEnemies = true;
            }

                        
            
        }

        // Set 'Speed' animation parameter based on the agent's velocity
        enemyRef.anim.SetFloat("Speed", enemyRef.agent.desiredVelocity.sqrMagnitude);
    }

    private void LookAtTarget()
    {
        // Make the enemy face the target
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void UpdatePath()
    {
        // Update the agent's path to the target position
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + enemyRef.pathUpdateDelay;
            enemyRef.agent.SetDestination(target.position);
        }
    }

    private void CheckAttack()
    {
        if (isAlive)
        {
            if (enemyRef.anim.GetBool("attack") && !alreadyAttacked)
            {
                alreadyAttacked = true;
                DealDamage();
            }
            else if (!enemyRef.anim.GetBool("attack"))
            {
                alreadyAttacked = false;
            }
        }
        else
        {
            // If the enemy is no longer alive, turn off the attack animation
            enemyRef.anim.SetBool("attack", false);
        }
    }



    private void DealDamage()
    {
        if (stats != null)
        {
            stats.TakeDamage(damage); 
        }
    }


    // Method to initiate spell casting
    private void TryCastSpell()
    {
        // Check if the enemy has enough mana and isn't currently casting a spell
        if (magic.HasEnoughMana() && !isCastingSpell)
        {
            isCastingSpell = true;
            lastSpellCastTime = Time.time;
            enemyRef.anim.SetBool("Spell", true); // Set the spell animation to true
            
        }
    }

    private void DestroySpawnedEnemies()
    {
        // Check if this enemy has spawned enemies using the EnemySpawner
        if (hasSpawnedEnemies && enemySpawner != null)
        {
            // Destroy all spawned enemies associated with this spawner
            enemySpawner.DestroySpawnedEnemies();
        }
    }

    private void SpellAttack()
    {        
        if (stats != null)
        {
            stats.TakeDamage(spellDamage);
        }
    }
}
