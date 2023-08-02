using DI;
using InGameStrings;
using UnityEngine;
using Values;

namespace Characters.Player
{
    public class CoinWallet : MonoBehaviour, IDIDependent
    {
        [Header("DI")]
        [DI(DIStrings.coinsCount)][SerializeField] private ValueEvent<int> _coinsCount;

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();
        }

        public void AddCoins(int value = 1)
        {
            _coinsCount.ChangeValue(_coinsCount.value + value);
        }
    }
}