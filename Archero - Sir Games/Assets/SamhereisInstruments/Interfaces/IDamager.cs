using UnityEngine;

namespace Interfaces
{
    public interface IDamager
    {
        public GameObject damagerGameobject { get; }
        public float Damage(IHealth damagable, float damage);
    }
}