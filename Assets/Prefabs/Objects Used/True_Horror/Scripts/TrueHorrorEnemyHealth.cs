using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueHorrorEnemyHealth : MonoBehaviour
{
    public float maxHP = 100;
    private float currentHP;
    public MiniMinionsEnemyBrain brain;
    public Animator anim;
    private bool isAlive = true;


    private void Start()
    {
        currentHP = maxHP;
        anim.applyRootMotion = false;     
    }

    

    public void TakeDamage(float damageAmount)
    {
        if (!isAlive)
        {
            return; // If already dead, do nothing
        }

        currentHP -= damageAmount;

        if (currentHP <= 0)
        {
            Die();
        }
        else {
            anim.SetTrigger("hit");
        }
    }

    private void Die()
    {
        isAlive = false;
        Destroy(brain);        
        anim.SetTrigger("die");
        
        
    }
}
