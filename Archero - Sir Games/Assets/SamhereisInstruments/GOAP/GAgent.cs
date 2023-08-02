using GOAP.GoapDataClasses;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class GAgent : MonoBehaviour, IInitializable
    {
        [field: SerializeField] public GAgentBaseSettings baseSettings { get; protected set; } = new GAgentBaseSettings();

        private void LateUpdate()
        {
            bool currentActionExistsAndRunning = baseSettings.currentAction != null && baseSettings.currentAction.baseSettings.running;

            if (currentActionExistsAndRunning)
            {
                TryCompleteAction();
                return;
            }

            bool plannerAndActionQueueEmpty = baseSettings.planner == null || baseSettings.actionQueue == null;

            if (plannerAndActionQueueEmpty)
            {
                CreatePlannerAndPopulateActionGoals();
            }

            bool actionQueueIsEmpty = baseSettings.actionQueue != null && baseSettings.actionQueue.Count == 0;

            if (actionQueueIsEmpty)
            {
                // Check if currentGoal is removable
                if (baseSettings.currentGoal.remove)
                {
                    // Remove it
                    baseSettings.goals.Remove(baseSettings.currentGoal);
                }
                // Set planner = null so it will trigger a new one
                baseSettings.planner = null;
            }

            bool actionQueueExists = baseSettings.actionQueue != null && baseSettings.actionQueue.Count > 0;

            if (actionQueueExists)
            {
                TryBegginNewAction();
            }
        }

        public virtual void Initialize()
        {
            GAction[] acts = GetComponentsInChildren<GAction>(true);
            foreach (GAction a in acts) baseSettings.actions.Add(a);
        }

        private void TryCompleteAction()
        {
            if (baseSettings.currentAction.TryComplete())
            {
                if (baseSettings.currentAction.IsCompleted())
                {
                    baseSettings.currentAction.SetIsRunning(false);
                    baseSettings.invoked = false;
                }
            }
        }

        private void TryBegginNewAction()
        {
            baseSettings.currentAction = baseSettings.actionQueue.Dequeue();

            if (baseSettings.currentAction.TryBeggin())
            {
                if (baseSettings.currentAction.baseSettings.target != null)
                {
                    baseSettings.currentAction.SetIsRunning(true);

                    baseSettings.destination = baseSettings.currentAction.baseSettings.target.transform.position;
                    Transform dest = baseSettings.currentAction.baseSettings.target.transform.Find("Destination");
                    if (dest != null) baseSettings.destination = dest.position;
                    if (baseSettings.currentAction.baseSettings.agent.destination != baseSettings.destination) baseSettings.currentAction.baseSettings.agent.SetDestination(baseSettings.destination);
                }
            }
            else
            {
                baseSettings.actionQueue = null;
            }
        }

        private void CreatePlannerAndPopulateActionGoals()
        {
            baseSettings.planner = new GPlanner();

            var sortedGoals = from entry in baseSettings.goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoals, int> sortedGoal in sortedGoals)
            {
                baseSettings.actionQueue = baseSettings.planner.Plan(baseSettings.actions, sortedGoal.Key.subGoals, baseSettings.localStates);

                if (baseSettings.actionQueue != null)
                {
                    baseSettings.currentGoal = sortedGoal.Key;
                    break;
                }
            }
        }

        [Serializable]
        public class GAgentBaseSettings
        {
            public Dictionary<SubGoals, int> goals = new Dictionary<SubGoals, int>();

            [field: SerializeField, Header("Debug")] public List<GAction> actions { get; protected internal set; } = new List<GAction>();
            [field: SerializeField] public GInventory inventory { get; private protected set; } = new GInventory();
            [field: SerializeField] public GoapStates localStates { get; protected internal set; } = new GoapStates();

            [field: SerializeField] public GAction currentAction { get; protected internal set; }

            [field: SerializeField] public SubGoals currentGoal;
            [field: SerializeField] public Vector3 destination { get; protected internal set; } = Vector3.zero;

            public GPlanner planner { get; protected internal set; }
            public Queue<GAction> actionQueue { get; protected internal set; }

            public bool invoked { get; protected internal set; } = false;
        }
    }
}