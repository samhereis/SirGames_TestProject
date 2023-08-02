using DI;
using InGameStrings;
using Interfaces;
using System;
using System.Collections;
using UnityEngine;
using Values;
using Random = UnityEngine.Random;

namespace Gameplay.Bullets
{
    public class SimpleGunBullet : ProjectileBase, IDamager, IDIDependent
    {
        public Action onCollided;

        public GameObject damagerGameobject { get; private set; }

        [Header("DI")]
        [DI(DIStrings.isGameActive)][SerializeField] private ValueEvent<bool> _isGameActive;

        [Header("Components")]
        [SerializeField] private ProjectileView _projectileView;
        [SerializeField] private Collider _collider;
        [SerializeField] Transform _mesh;

        [Header("Settings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _angleDifference;
        [SerializeField] private float _damage;
        [SerializeField] private float _selfPutinToPoolTime = 3;

        [Header("Debug")]
        [SerializeField] private Vector3 _angle;
        [SerializeField] private bool _hasCollided = false;

        private void OnValidate()
        {
            if (_projectileView == null) _projectileView = GetComponentInChildren<ProjectileView>();
        }

        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            _hasCollided = false;

            _mesh?.gameObject.SetActive(true);

            _projectileView?.Init();
            onCollided += OnEnd;

            _angle = new Vector3(Random.Range(-_angleDifference, _angleDifference), Random.Range(-_angleDifference, _angleDifference), _speed);

            StartCoroutine(AutoEnd());
        }

        private void OnDisable()
        {
            ResetVelocity();

            onCollided -= OnEnd;
            _projectileView?.OnEnd();
        }

        private void Update()
        {
            if (_hasCollided == true) { return; }

            if (_isGameActive.value == true)
            {
                transform.position += transform.forward * _speed * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasCollided == true) return;

            onCollided?.Invoke();
            _mesh?.gameObject.SetActive(false);

            _hasCollided = true;

            if (other.TryGetComponent(out IHealth damagable))
            {
                Damage(damagable, _damage);
            }
        }

        public override void Initialize()
        {
            (this as IDIDependent).LoadDependencies();

            base.Initialize();

            damagerGameobject = gameObject;
        }

        private void OnEnd()
        {
            onCollided -= OnEnd;

            _projectileView?.OnEnd(() =>
            {
                _pooling.PutIn(this);
            });
        }

        private IEnumerator AutoEnd()
        {
            yield return new WaitForSeconds(_selfPutinToPoolTime);

            while (_isGameActive.value == false)
            {
                yield return null;
            }

            OnEnd();
        }

        private void ResetVelocity()
        {
            transform.position = Vector3.zero;
        }

        public float Damage(IHealth damagable, float damage)
        {
            return damagable.TakeDamage(_damage, this);
        }
    }
}