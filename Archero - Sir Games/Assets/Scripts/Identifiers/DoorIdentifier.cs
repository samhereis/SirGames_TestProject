using DG.Tweening;
using DI;
using Events;
using InGameStrings;
using Interfaces;
using UnityEngine;

namespace Identifiers
{
    public class DoorIdentifier : MonoBehaviour, IDIDependent, ISubscribesToEvents
    {
        [Header("DI")]
        [DI(DIStrings.onAllEnemiesKilled)][SerializeField] private EventWithNoParameters _onAllEnemiesKilled;
        [DI(DIStrings.onWin)][SerializeField] private EventWithNoParameters _onWin;

        [Header("Components")]
        [SerializeField] private Transform _leftSide;
        [SerializeField] private Transform _rightSide;

        private bool _isHandled = false;

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();

            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isHandled == false)
            {
                if (other.TryGetComponent<PlayerIdentifier>(out PlayerIdentifier player) == true)
                {
                    _isHandled = true;

                    UnsubscribeFromEvents();
                    _onWin?.Invoke();
                }
            }
        }

        public void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _onAllEnemiesKilled.AddListener(Open);
        }

        public void UnsubscribeFromEvents()
        {
            _onAllEnemiesKilled.RemoveListener(Open);
        }

        private void Open()
        {
            _leftSide.DOMoveX(_leftSide.position.x - 1f, 1f).SetEase(Ease.OutBack);
            _rightSide.DOMoveX(_rightSide.position.x + 1f, 1f).SetEase(Ease.OutBack);
        }
    }
}