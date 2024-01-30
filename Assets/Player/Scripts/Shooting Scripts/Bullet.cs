using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy;
    [HideInInspector] public WeaponManager weapon;
        
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(weapon.damage);
        }

        if (collision.gameObject.GetComponentInParent<TrueHorrorEnemyHealth>())
        {
            TrueHorrorEnemyHealth minionHealth = collision.gameObject.GetComponentInParent<TrueHorrorEnemyHealth>();
            minionHealth.TakeDamage(weapon.damage);
        }
        
        Destroy(this.gameObject);        
        
    }

    
}
