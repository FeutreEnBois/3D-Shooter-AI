using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float dieForce;
    public float dieForceY = 1.0f;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Ragdoll ragdoll;
    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;
    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<UIHealthBar>();
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
        healthBar.SetHealthBarPercentage(currentHealth/maxHealth);
        if (currentHealth <= 0)
        {
            Die(direction);
        }

        blinkTimer = blinkDuration;
    }

    private void Die(Vector3 direction)
    {
        ragdoll.ActivateRagdoll();
        direction.y = dieForceY;
        ragdoll.ApplyForce(direction * dieForce);
        healthBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}
