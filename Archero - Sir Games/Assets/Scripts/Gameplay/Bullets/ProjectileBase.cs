using Interfaces;
using Newtonsoft.Json.Bson;
using Pooling;
using UnityEngine;

namespace Gameplay.Bullets
{
    public abstract class ProjectileBase : MonoBehaviour, IInitializable
    {
        [SerializeField] protected BulletPooling_SO _pooling;

        public virtual void Initialize()
        {

        }
    }
}