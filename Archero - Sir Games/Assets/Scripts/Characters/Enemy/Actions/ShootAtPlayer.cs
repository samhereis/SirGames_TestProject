using Characters.Enemy;
using DG.Tweening;
using DI;
using Helpers;
using Identifiers;
using InGameStrings;
using Interfaces;
using SO.GOAP;
using UnityEngine;
using Values;

namespace GOAP.Actions
{
    public class ShootAtPlayer : GAction, IDamager, IDIDependent
    {
        public GameObject damagerGameobject => gameObject;

        [Header("DI")]
        [DI(DIStrings.isGameActive)][SerializeField] private ValueEvent<bool> _isGameActive;

        [Header("Components")]
        [SerializeField] private EnemyAgent _enemyAgent;

        [Header("Settings")]
        [SerializeField] private float _damage = 5f;
        [SerializeField] private float _shootRateMin = 1f;
        [SerializeField] private float _shootRateMax = 3f;
        [SerializeField] private float _distanceToPlayerToShoot = 10;

        [Header("Goap Strings")]
        [SerializeField] private GOAPString _isPlayerVisible;

        [Header("Debug")]
        [SerializeField] private PlayerIdentifier _playerIdentifier;

        private bool _canShoot = true;

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();
        }

        private void OnDestroy()
        {
            _enemyAgent.animationAgent.transform.DOKill();
        }

        public override bool IsAchievable()
        {
            if (CanRun() == false) return false;

            if (_playerIdentifier == null)
            {
                _playerIdentifier = FindAnyObjectByType<PlayerIdentifier>();
            }

            if (_playerIdentifier == null) return false;
            if (_playerIdentifier.TryGet<PlayerHealth>().isAlive == false) return false;

            baseSettings.target = _playerIdentifier.gameObject;

            return baseSettings.target != null;
        }

        public override bool TryBeggin()
        {
            return IsPlayerVisible();
        }

        public override bool IsCompleted()
        {
            return _playerIdentifier.TryGet<PlayerHealth>().isAlive == false;
        }

        public override bool TryComplete()
        {
            if (CanRun() == false)
            {
                _enemyAgent.Stop();
                SetIsRunning(false);
                return false;
            }

            bool isCompleted = false;

            if (IsPlayerVisible())
            {
                _enemyAgent.Stop();

                var health = _playerIdentifier.TryGet<PlayerHealth>();

                Damage(health, _damage);

                isCompleted = health.isAlive == false;

                if (isCompleted == true)
                {
                    _enemyAgent.Stop();
                }
            }
            else
            {
                _enemyAgent.Stop();
                _enemyAgent.baseSettings.localStates.RemoveState(_isPlayerVisible);

                SetIsRunning(false);
            }

            return isCompleted;
        }

        public float Damage(IHealth damagable, float damage)
        {
            _enemyAgent.animationAgent.transform.DOLookAt(baseSettings.target.transform.position, 0.25f);

            if (_canShoot == false) return damagable.currentHealth;
            _canShoot = false;

            _enemyAgent.animationAgent.PlayAnimation("Shoot");

            ResetCanShoot();

            return damagable.currentHealth;
        }

        private bool CanRun()
        {
            bool canComplete = true;

            if (_enemyAgent.identifier.TryGet<EnemyHealth>().isAlive == false)
            {
                canComplete = false;
            }

            if (_playerIdentifier != null)
            {
                if (_playerIdentifier.TryGet<PlayerHealth>().isAlive == false)
                {
                    canComplete = false;
                }
            }

            if (_isGameActive.value == false)
            {
                canComplete = false;
            }

            return canComplete;
        }

        private bool IsPlayerVisible()
        {
            if (baseSettings.target == null) return false;
            if (Vector3.Distance(baseSettings.target.transform.position, transform.position) > _distanceToPlayerToShoot) return false;

            return _enemyAgent.CanSee(baseSettings.target.transform, _distanceToPlayerToShoot);
        }

        private async void ResetCanShoot()
        {
            var shootRate = Random.Range(_shootRateMin, _shootRateMax);

            await AsyncHelper.Delay(shootRate);

            _canShoot = true;
        }
    }
}