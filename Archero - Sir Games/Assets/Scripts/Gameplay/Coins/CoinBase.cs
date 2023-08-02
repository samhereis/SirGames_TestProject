using Helpers;
using Identifiers;
using Interfaces;
using System.Threading;
using UnityEngine;

namespace Gameplay.Coins
{
    public class CoinBase : MonoBehaviour, IInitializable<IdentifierBase>
    {
        [Header("Settings")]
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 10f;

        [Header("Debug")]
        [SerializeField] private IdentifierBase _target;

        private bool _canFlyToTarget = false;

        private CancellationTokenSource _onDestroyCancellationTokenSource = new CancellationTokenSource();

        private void OnEnable()
        {
            _canFlyToTarget = false;
            SetCanFly();
        }

        private void OnDisable()
        {
            _canFlyToTarget = false;
        }

        private void OnDestroy()
        {
            _onDestroyCancellationTokenSource.Cancel();
        }

        private void Update()
        {
            transform.Rotate(new Vector3(0f, _rotationSpeed * Time.fixedDeltaTime, 0f));
        }

        private void FixedUpdate()
        {
            if (_canFlyToTarget == true)
            {
                FlyToTarget();
            }
        }

        protected async virtual void SetCanFly()
        {
            await AsyncHelper.Delay(1f);

            if (_onDestroyCancellationTokenSource.IsCancellationRequested == false) _canFlyToTarget = true;
        }

        protected virtual void FlyToTarget()
        {
            if (_target != null) transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _moveSpeed * Time.fixedDeltaTime);
        }

        public void Initialize(IdentifierBase target)
        {
            _target = target;
        }
    }
}