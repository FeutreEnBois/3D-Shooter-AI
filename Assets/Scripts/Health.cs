using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    SkinnedMeshRenderer skinnedMeshRenderer;
    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;
    // Start is called before the first frame update
    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;

        var rigiBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rig in rigiBodies)
        {
            Hitbox hitbox = rig.gameObject.AddComponent<Hitbox>();
            hitbox.health = this;
            if(hitbox.gameObject != gameObject)
            {
                hitbox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
            }
        }

        OnStart();
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        if (healthBar)
        {
            healthBar.SetHealthBarPercentage(currentHealth/maxHealth);
        }
        OnDamage(direction);
        if (currentHealth <= 0)
        {
            Die(direction);
        }

        blinkTimer = blinkDuration;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    private void Die(Vector3 direction)
    {
        OnDeath(direction);
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }

    protected virtual void OnStart() { }
    protected virtual void OnDeath(Vector3 direction) { }
    protected virtual void OnDamage(Vector3 direction) { }
}
