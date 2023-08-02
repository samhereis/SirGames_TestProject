using Identifiers;
using System;
using UnityEngine;

namespace Interfaces
{
    public interface IHealth
    {
        public Action<float> onHealthChanged { get; set; }
        public Action<IHealth> onDie { get; set; }

        public IdentifierBase damagedGameobject { get; }
        public float currentHealth { get; }
        public float maxHealth { get; }
        public bool isAlive { get; }

        public float TakeDamage(float damage, IDamager damager);
    }
}