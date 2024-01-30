using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMinionsEnemyBrain : MonoBehaviour
{
    public GameObject target;

    private MiniMinionsEnemyReferences enemyRef;
    private float pathUpdateDeadline;
    private float fightingDistance;
    private bool isAlive = true;

    // Attacking
    private bool alreadyAttacked;
    private float damage = 3f;

    private void Awake()
    {
        enemyRef = GetComponent<MiniMinionsEnemyReferences>();
    }

    void Start()
    {
        fightingDistance = enemyRef.agent.stoppingDistance;
        FindAndSetTargetWithTag("Player");
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        if (target != null)
        {
            bool inRange = Vector3.Distance(transform.position, target.transform.position) <= fightingDistance;

            if (inRange)
            {
                LookAtTarget();

                if (!alreadyAttacked)
                {
                    enemyRef.anim.SetBool("attack", true); // Set attack animation

                    PlayerStats stats = target.GetComponent<PlayerStats>();
                    stats.TakeDamage(damage);

                    alreadyAttacked = true; // Set the flag
                }
            }
            else
            {
                alreadyAttacked = false; // Reset the flag
                enemyRef.anim.SetBool("attack", false); // Reset attack animation
                UpdatePath();
            }

            enemyRef.anim.SetFloat("speed", enemyRef.agent.desiredVelocity.sqrMagnitude);
        }
    }

    private void LookAtTarget()
    {
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void UpdatePath()
    {
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + enemyRef.pathUpdateDelay;
            enemyRef.agent.SetDestination(target.transform.position);
        }
    }

    private void FindAndSetTargetWithTag(string tag)
    {
        GameObject targetObject = GameObject.FindWithTag(tag);
        if (targetObject != null)
        {
            target = targetObject;
        }
        else
        {
            Debug.LogWarning("No object found with tag: " + tag);
        }
    }
}
