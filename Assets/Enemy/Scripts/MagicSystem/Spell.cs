using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    public SpellScriptableObject spellToCast;

    private SphereCollider myCollider;
    private Rigidbody rb;


    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = spellToCast.SpellRadius;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        Destroy(this.gameObject, spellToCast.LifeTime);
    }


    private void Update()
    {
        if (spellToCast.speed > 0) transform.Translate(Vector3.forward * spellToCast.speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
        }
        Destroy(this.gameObject);
    }
}
