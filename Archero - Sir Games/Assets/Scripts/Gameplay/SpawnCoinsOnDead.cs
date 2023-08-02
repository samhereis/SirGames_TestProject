using Gameplay.Coins;
using Helpers;
using Identifiers;
using Interfaces;
using UnityEngine;

namespace Gameplay
{
    public class SpawnCoinsOnDead : MonoBehaviour, ISubscribesToEvents
    {
        [Header("Prefabs")]
        [SerializeField] private CoinBase _coinPrefab;

        [Header("Settings")]
        [SerializeField] private int _coinsCountMin = 3;
        [SerializeField] private int _coinsCountMax = 5;

        private IHealth _health;

        private void Start()
        {
            _health = GetComponent<IHealth>();

            if (_health != null)
            {
                SubscribeToEvents();
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        public void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _health.onDie += OnEnemyDie;
        }

        public void UnsubscribeFromEvents()
        {
            _health.onDie -= OnEnemyDie;
        }

        private async void OnEnemyDie(IHealth health)
        {
            PlayerIdentifier playerIdentifier = FindAnyObjectByType<PlayerIdentifier>();

            int coinsCount = Random.Range(_coinsCountMin, _coinsCountMax + 1);

            for (int i = 0; i < coinsCount; i++)
            {
                CoinBase coin = Instantiate(_coinPrefab, SpawnNearPositionUsingNavmesh.GetNearPosition(transform.position, 1f, 5f), Quaternion.identity);
                coin.Initialize(playerIdentifier);

                await AsyncHelper.Delay();
            }
        }
    }
}