using Agents;
using DG.Tweening;
using DI;
using Events;
using InGameStrings;
using Interfaces;
using PlayerInputHolder;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerMovement : MonoBehaviour, IDIDependent, ISubscribesToEvents
    {
        public Action<bool> onMovingStateChanged;

        [Header("DI")]
        [DI(DIStrings.inputHolder)][SerializeField] private Input_SO _inputActions;
        [DI(DIStrings.onGameStart)][SerializeField] private EventWithNoParameters _onGameStart;

        [Header("Components")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private AnimationAgent _animationAgent;
        [SerializeField] private PlayerHealth _playerHealth;

        [Header("Settings")]
        [SerializeField] private float _movementSpeed;

        private Vector2 _movementVelocity;
        private bool _isGameStarted = false;

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
            _onGameStart.RemoveListener(OnGameStart);
            UnsubscribeFromEvents();
        }

        private void FixedUpdate()
        {
            if (_isGameStarted == false) return;
            if(_playerHealth.isAlive == false) return;

            var movementVector = new Vector3(_movementVelocity.x, 0, _movementVelocity.y);
            _rigidbody.velocity = movementVector;
        }

        private void OnGameStart()
        {
            _isGameStarted = true;
            SubscribeToEvents();
        }

        public void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _inputActions.input.Player.Move.started += OnMoveStarted;
            _inputActions.input.Player.Move.performed += OnMove;
            _inputActions.input.Player.Move.canceled += OnMoveStopped;

            _playerHealth.onDie += OnDie;
        }

        public void UnsubscribeFromEvents()
        {
            _inputActions.input.Player.Move.started -= OnMoveStarted;
            _inputActions.input.Player.Move.performed -= OnMove;
            _inputActions.input.Player.Move.canceled -= OnMoveStopped;

            _playerHealth.onDie -= OnDie;
        }

        private void OnMoveStarted(InputAction.CallbackContext context)
        {
            onMovingStateChanged?.Invoke(true);
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 movementInput = context.ReadValue<Vector2>();

            Move(movementInput, 1);

            var movementVector = new Vector3(_movementVelocity.x, 0, _movementVelocity.y);
            _animationAgent.transform.DORotateQuaternion(Quaternion.LookRotation(movementVector, Vector3.up), 0.5f);
        }

        private void OnMoveStopped(InputAction.CallbackContext context)
        {
            Move(context.ReadValue<Vector2>(), 0);

            onMovingStateChanged?.Invoke(false);
        }

        private void Move(Vector2 velocity, float speed)
        {
            if (speed > 0)
            {
                _movementVelocity = velocity.normalized * _movementSpeed;
            }
            else
            {
                _movementVelocity = Vector2.zero;
            }

            _animationAgent.PlayAnimation("Movement");
            _animationAgent.animator.SetFloat("Speed", speed);
        }

        private void OnDie(IHealth health)
        {
            UnsubscribeFromEvents();
        }
    }
}