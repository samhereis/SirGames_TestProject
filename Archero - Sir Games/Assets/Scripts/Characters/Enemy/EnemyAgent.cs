using Agents;
using DI;
using Events;
using GOAP;
using GOAP.GoapDataClasses;
using GOAP.View;
using Identifiers;
using InGameStrings;
using Interfaces;
using SO.GOAP;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Enemy
{
    public class EnemyAgent : GAgent, IDIDependent, ISubscribesToEvents
    {
        [SerializeField] private List<GoalView> _goapGoals = new List<GoalView>();
        [SerializeField] private List<LocalStateView> _localStates = new List<LocalStateView>();

        [Header("DI")]
        [DI(DIStrings.onGameStart)][SerializeField] private EventWithNoParameters _onGameStart;

        [Header("Components")]
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [field: SerializeField] public AnimationAgent animationAgent { get; private set; }
        [field: SerializeField] public IdentifierBase identifier { get; private set; }

        [Header("Settings")]
        [SerializeField] private float _moveSpeed = 1.5f;
        [SerializeField] private LayerMask _raycastLM;

        private void Awake()
        {
            identifier = GetComponent<IdentifierBase>();
        }

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();

            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        public void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _onGameStart.AddListener(Initialize);
        }

        public void UnsubscribeFromEvents()
        {
            _onGameStart.RemoveListener(Initialize);
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var goal in _goapGoals)
            {
                var subGoals = new Dictionary<GOAPString, int>();

                foreach (var subgoal in goal.subgoal)
                {
                    subGoals.Add(subgoal.subgoal, subgoal.cost);
                }

                baseSettings.goals.Add(new SubGoals(subGoals, false), goal.priority);
            }

            foreach (var state in _localStates)
            {
                baseSettings.localStates.SetState(state.subgoal, state.cost);
            }
        }

        public void GoTo(Vector3 position)
        {
            SetDestination(position, _moveSpeed);
        }

        private void SetDestination(Vector3 position, float speed)
        {
            animationAgent.animator.SetFloat("Speed", 1);

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(position);
            _navMeshAgent.speed = speed;
        }

        public void Stop()
        {
            animationAgent.animator.SetFloat("Speed", 0);

            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
        }

        public bool CanSee(Transform target, float range)
        {
            bool canSee = false;

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, target.position - transform.position, out hit, range, _raycastLM))
            {
                if (hit.collider.gameObject.TryGetComponent(out PlayerIdentifier playerIdentifier))
                {
                    canSee = true;
                }
            }

            return canSee;
        }
    }
}