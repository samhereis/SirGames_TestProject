using Characters.Enemy;
using DG.Tweening;
using DI;
using Identifiers;
using InGameStrings;
using SO.GOAP;
using UnityEngine;
using Values;

namespace GOAP.Actions
{
    public class GoToPlayer : GAction, IDIDependent
    {
        public GameObject damagerGameobject => gameObject;

        [Header("DI")]
        [DI(DIStrings.isGameActive)][SerializeField] private ValueEvent<bool> _isGameActive;

        [Header("Components")]
        [SerializeField] private EnemyAgent _enemyAgent;

        [Header("Settings")]
        [SerializeField] private float _seeRange = 15;
        [SerializeField] private float _targetDistance = 10;

        [Header("Goap Strings")]
        [SerializeField] private GOAPString _isPlayerVisible;

        [Header("Debug")]
        [SerializeField] private PlayerIdentifier _playerIdentifier;
        [SerializeField] private bool _hasEverSeenPlayer = false;

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

            baseSettings.target = _playerIdentifier.gameObject;

            if (_playerIdentifier != null)
            {
                return _playerIdentifier.TryGet<PlayerHealth>().isAlive == true;
            }

            bool canStart = true;

            if (_hasEverSeenPlayer == false)
            {
                canStart = _enemyAgent.CanSee(baseSettings.target.transform, _seeRange);
            }

            return canStart;
        }

        public override bool TryBeggin()
        {

            return _playerIdentifier != null;
        }

        public override bool IsCompleted()
        {
            bool isCompleted = _enemyAgent.CanSee(baseSettings.target.transform, _seeRange) && GetDistance() < _targetDistance;

            if (isCompleted == true)
            {
                _enemyAgent.baseSettings.localStates.SetState(_isPlayerVisible, 0);
            }

            return isCompleted;
        }

        public override bool TryComplete()
        {
            if (CanRun() == false)
            {
                _enemyAgent.Stop();
                SetIsRunning(false);
                return false;
            }

            bool canSee = GetDistance() < _targetDistance && _enemyAgent.CanSee(baseSettings.target.transform, _seeRange);

            if (canSee == true)
            {
                _hasEverSeenPlayer = true;
            }

            bool shouldGo = canSee;

            if (_hasEverSeenPlayer == false)
            {
                if (canSee == false)
                {
                    _enemyAgent.Stop();
                    return false;
                }
            }
            else
            {
                shouldGo = true;
            }

            if (shouldGo == true)
            {
                _enemyAgent.GoTo(_playerIdentifier.transform.position);
                _enemyAgent.animationAgent.transform.DOLocalRotate(Vector3.zero, 0.25f);
            }
            else
            {
                _enemyAgent.Stop();
            }

            return canSee;
        }

        private float GetDistance()
        {
            return Vector3.Distance(_enemyAgent.transform.position, baseSettings.target.transform.position);
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
    }
}