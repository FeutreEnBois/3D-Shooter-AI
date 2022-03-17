using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    Ragdoll ragdoll;
    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;

        var rigiBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rig in rigiBodies)
        {
            Hitbox hitbox = rig.gameObject.AddComponent<Hitbox>();
            hitbox.health = this;
        }
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        if(currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ragdoll.ActivateRagdoll();
    }
}
