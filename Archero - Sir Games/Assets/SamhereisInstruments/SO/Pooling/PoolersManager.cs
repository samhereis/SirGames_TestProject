using Gameplay.Bullets;
using Pooling;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.World.Helpers
{
    public sealed class PoolersManager : MonoBehaviour
    {
        [SerializeField] private List<PoolerBase<ProjectileBase>> _poolers;

        private void Awake()
        {
            foreach (var pooler in _poolers)
            {
                Transform parent = new GameObject(pooler.name).transform;
                parent.parent = transform;

                pooler?.Initialize(parent);
                pooler?.Initialize();
            }
        }

        private void OnDisable()
        {
            foreach (var pooler in _poolers)
            {
                Destroy(transform.Find(pooler.name).gameObject);
                pooler?.Clear();
            }
        }
    }
}