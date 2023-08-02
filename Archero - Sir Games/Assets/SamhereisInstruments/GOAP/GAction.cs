using GOAP.GoapDataClasses;
using SO.GOAP;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public abstract class GAction : MonoBehaviour
    {
        [Serializable]
        public class GActionBaseSettings
        {
            public Dictionary<GOAPString, int> preConditions { get; protected internal set; } = new Dictionary<GOAPString, int>();
            public Dictionary<GOAPString, int> afterEffects { get; protected internal set; } = new Dictionary<GOAPString, int>();

            [field: SerializeField] public NavMeshAgent agent { get; protected internal set; }

            [field: SerializeField, Header("Settings")] public GOAPActionName actionName { get; protected internal set; }
            [field: SerializeField] public float cost { get; protected internal set; } = 1.0f;
            [field: SerializeField] public float duration { get; protected internal set; } = 0.0f;

            [field: SerializeField, Header("Goap States")] public GoapState[] preConditionsInspector { get; protected internal set; }
            [field: SerializeField] public GoapState[] afterEffectsInspector { get; protected internal set; }
            [field: SerializeField] public GoapStates localStates { get; protected internal set; }

            [field: SerializeField, Header("Debug")] public bool running { get; protected internal set; } = false;
            [field: SerializeField] public GInventory inventory { get; protected internal set; }
            [field: SerializeField] public GameObject target { get; protected internal set; }
        }

        [field: SerializeField] public GActionBaseSettings baseSettings { get; set; } = new GActionBaseSettings();

        protected virtual void Awake()
        {
            baseSettings.agent = this.gameObject.GetComponent<NavMeshAgent>();

            if (baseSettings.preConditionsInspector != null)
            {
                foreach (GoapState w in baseSettings.preConditionsInspector)
                {
                    baseSettings.preConditions.Add(w.key, w.value);
                }
            }

            if (baseSettings.afterEffectsInspector != null)
            {
                foreach (GoapState w in baseSettings.afterEffectsInspector)
                {
                    baseSettings.afterEffects.Add(w.key, w.value);
                }
            }

            baseSettings.inventory = this.GetComponent<GAgent>().baseSettings.inventory;
            baseSettings.localStates = this.GetComponent<GAgent>().baseSettings.localStates;
        }


        public bool IsAhievableGiven(Dictionary<GOAPString, int> conditions)
        {
            return baseSettings.preConditions.All(x => conditions.ContainsKey(x.Key));
        }

        public virtual void SetIsRunning(bool isRunning)
        {
            baseSettings.running = isRunning;
        }

        public abstract bool IsAchievable();
        public abstract bool TryBeggin();
        public abstract bool TryComplete();
        public abstract bool IsCompleted();
    }
}