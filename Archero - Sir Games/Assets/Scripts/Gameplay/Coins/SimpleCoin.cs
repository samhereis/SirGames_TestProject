using Characters.Player;
using Identifiers;
using UnityEngine;

namespace Gameplay.Coins
{
    public class SimpleCoin : CoinBase
    {
        [Header("Settings")]
        [SerializeField] private int _coinsCount = 2;

        [Header("Debug")]
        [SerializeField] private bool _isPickedUp = false;

        private void OnTriggerEnter(Collider other)
        {
            if(_isPickedUp == true) return;

            if (other.TryGetComponent(out PlayerIdentifier playerIdentifier) == true)
            {
                playerIdentifier.TryGet<CoinWallet>()?.AddCoins(_coinsCount);

                _isPickedUp = true;

                Destroy(gameObject);
            }
        }
    }
}