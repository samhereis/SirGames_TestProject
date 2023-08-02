using Agents;
using DG.Tweening;
using DI;
using Events;
using Helpers;
using Identifiers;
using InGameStrings;
using Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Values;

namespace Characters.Player
{
    public class PlayerShooting : MonoBehaviour, IDIDependent, ISubscribesToEvents, IDamager
    {
        public GameObject damagerGameobject => gameObject;

        [Header("DI")]
        [DI(DIStrings.onGameStart)][SerializeField] private EventWithNoParameters _onGameStart;
        [DI(DIStrings.isGameActive)][SerializeField] private ValueEvent<bool> _isGameActive;

        [Header("Components")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerIdentifier _playerIdentifier;
        [SerializeField] private AnimationAgent _animationAgent;
        [SerializeField] private PlayerHealth _playerHealth;

        [Header("Settings")]
        [SerializeField] private float _shootRange = 10f;
        [SerializeField] private float _rotationDuration = 5f;
        [SerializeField] private float _shootRate = 1f;
        [SerializeField] private LayerMask _raycastLM;

        [Header("Debug")]
        [SerializeField] private List<IHealth> _allEnemies = new List<IHealth>();
        [SerializeField] private bool _canShoot = false;
        [SerializeField] private Transform _lookTarget;

        private CancellationTokenSource _onDestroyCTS = new CancellationTokenSource();

        private void Awake()
        {
            if (_playerHealth == null) _playerHealth = GetComponent<PlayerHealth>();
        }

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();

            _onGameStart.AddListener(OnGameStart);
        }

        private void OnDestroy()
        {
            _animationAgent.transform.DOKill();

            _onGameStart.RemoveListener(OnGameStart);
            UnsubscribeFromEvents();

            _onDestroyCTS.Cancel();
        }

        private void Update()
        {
            if (_canShoot == false) return;
            if (_lookTarget == null) return;
            if (_isGameActive.value == false) return;
            if (_playerHealth.isAlive == false) return;

            _animationAgent.transform.DOLookAt(_lookTarget.position, _rotationDuration);
        }

        private void OnGameStart()
        {
            foreach (var enemy in FindObjectsByType<EnemyIdentifier>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList())
            {
                if (enemy.TryGetComponent(out IHealth health))
                {
                    _allEnemies.Add(health);
                }
            }

            SubscribeToEvents();
        }

        public void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _playerMovement.onMovingStateChanged += OnMovingStateChanged;

            _playerHealth.onDie += OnDie;
        }

        public void UnsubscribeFromEvents()
        {
            _playerMovement.onMovingStateChanged -= OnMovingStateChanged;

            _playerHealth.onDie -= OnDie;
        }

        private void OnMovingStateChanged(bool isMoving)
        {
            if (_isGameActive.value == false) return;
            _canShoot = isMoving == false;

            if (_canShoot == false) return;

            TryConstantlyShoot();
        }

        private async void TryConstantlyShoot()
        {
            IHealth health = null;

            while (_canShoot == true && _onDestroyCTS.IsCancellationRequested == false)
            {
                if (_playerIdentifier.TryGet<PlayerHealth>().isAlive == false)
                {
                    _canShoot = false;
                    return;
                }

                if (_isGameActive.value == true)
                {
                    health = await TryGetEnemy();

                    if (health == null)
                    {
                        await AsyncHelper.Delay(1f);
                        continue;
                    }

                    float distance = Vector3.Distance(transform.position, health.damagedGameobject.transform.position);

                    if (distance > _shootRange)
                    {
                        await AsyncHelper.Delay(1f);
                        continue;
                    }

                    _lookTarget = health.damagedGameobject.transform;

                    _animationAgent.PlayAnimation("Shoot");
                }

                await AsyncHelper.Delay(_shootRate);
            }
        }

        private async Task<IHealth> TryGetEnemy()
        {
            SortEnemiesByDistance();
            if (_allEnemies.Count == 0) return null;

            IHealth health = null;

            foreach (var enemy in _allEnemies)
            {
                if (_onDestroyCTS.IsCancellationRequested) return null;

                if (Physics.Raycast(transform.position + Vector3.up, enemy.damagedGameobject.transform.position - transform.position, out RaycastHit hit, _shootRange, _raycastLM))
                {
                    if (hit.collider.gameObject.TryGetComponent(out EnemyIdentifier playerIdentifier))
                    {
                        health = enemy;
                        break;
                    }
                }

                await AsyncHelper.Delay();
            }

            return health;
        }

        private void SortEnemiesByDistance()
        {
            if (_allEnemies.Count == 0) return;

            _allEnemies.RemoveAll(x => x.isAlive == false);
            _allEnemies = _allEnemies.OrderBy(x => Vector3.Distance(transform.position, x.damagedGameobject.transform.position)).ToList();
        }

        public float Damage(IHealth damagable, float damage)
        {
            return damagable.TakeDamage(damage, this);
        }

        private void OnDie(IHealth health)
        {
            UnsubscribeFromEvents();
        }
    }
}