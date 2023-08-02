using Agents;
using Pooling;
using UnityEngine;

namespace Characters
{
    public abstract class BowShooterBase : MonoBehaviour
    {
        [SerializeField] private BulletPooling_SO _bowPooling;
        [SerializeField] private AnimationAgent _animationAgent;

        [Header("Settings")]
        [SerializeField] private string _shootCallback = "ShootArrow";

        private void Start()
        {
            _animationAgent.onAnimationCallback += OnShoot;
        }

        private void OnDestroy()
        {
            _animationAgent.onAnimationCallback -= OnShoot;
        }

        private void OnShoot(string callbackName)
        {
            if (callbackName == _shootCallback)
            {
                Shoot();
            }
        }

        public virtual void Shoot()
        {
            var arrow = _bowPooling.PutOff(transform.position, transform.rotation);
        }
    }
}