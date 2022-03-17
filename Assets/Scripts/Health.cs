using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    SkinnedMeshRenderer skinnedMeshRenderer;
    AiAgent agent;
    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<AiAgent>();
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
        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateId.Death);
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}
