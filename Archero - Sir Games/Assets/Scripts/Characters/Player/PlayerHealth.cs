using Agents;
using Identifiers;
using Interfaces;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public Action<float> onHealthChanged { get; set; }
    public Action<IHealth> onDie { get; set; }

    [field: SerializeField] public IdentifierBase damagedGameobject { get; private set; }

    [field: SerializeField] public float currentHealth { get; private set; } = 100;

    [field: SerializeField] public float maxHealth { get; private set; } = 100;

    [field: SerializeField] public bool isAlive { get; private set; } = true;

    [Header("Components")]
    [SerializeField] private AnimationAgent _animationAgent;

    private void Awake()
    {
        isAlive = true;

        if (_animationAgent == null) _animationAgent = GetComponentInChildren<AnimationAgent>(true);
        damagedGameobject = gameObject.GetComponent<IdentifierBase>();
    }

    public float TakeDamage(float damage, IDamager damager)
    {
        if (isAlive == true)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            onHealthChanged?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        return currentHealth;
    }

    private void Die()
    {
        isAlive = false;

        _animationAgent?.PlayAnimation("Die");

        onDie?.Invoke(this);
    }
}