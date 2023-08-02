using Identifiers;
using UnityEngine;

namespace Gameplay
{
    public class MainCamera : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _lerpSpeed = 1f;

        [Header("Debug")]
        [SerializeField] private PlayerIdentifier _playerIdentifier;

        private void Start()
        {
            if (GetPlayer(out PlayerIdentifier player))
            {
                transform.position = GetTargetPosition();
            }
        }

        private void Update()
        {
            if (GetPlayer(out PlayerIdentifier player))
            {
                GoToTarget(GetTargetPosition(), _lerpSpeed);
            }
        }

        private void GoToTarget(Vector3 target, float speed)
        {
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        }

        private Vector3 GetTargetPosition()
        {
            return new Vector3(0, 0, _playerIdentifier.transform.position.z);
        }

        private bool GetPlayer(out PlayerIdentifier playerIdentifier)
        {
            if (_playerIdentifier == null)
            {
                _playerIdentifier = FindObjectOfType<PlayerIdentifier>(true);
            }

            playerIdentifier = _playerIdentifier;

            return playerIdentifier != null;
        }
    }
}