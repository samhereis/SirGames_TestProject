using System;
using UnityEngine;

namespace Events
{
    [Serializable]
    public class EventWithNoParameters
    {
        public Action onInvoke { get; private set; }

        [field: SerializeField] public string eventName { get; private set; }
        [field: SerializeField] public int currentSubscribedObjectsCount { get; private set; }
        [field: SerializeField] public int timesInvoked { get; private set; }

        public EventWithNoParameters(string eventName)
        {
            this.eventName = eventName;
        }

        public void AddListener(Action action)
        {
            onInvoke += action;
            UpdateCurrentSubscribedObjectsCount();
        }

        public void RemoveListener(Action action)
        {
            onInvoke -= action;
            UpdateCurrentSubscribedObjectsCount();
        }

        public void Clear()
        {
            onInvoke = null;
        }

        public void Invoke()
        {
            onInvoke?.Invoke();
            timesInvoked++;
        }

        private void UpdateCurrentSubscribedObjectsCount()
        {
            if (onInvoke == null)
            {
                currentSubscribedObjectsCount = 0;
            }
            else
            {
                currentSubscribedObjectsCount = onInvoke.GetInvocationList().Length;
            }
        }
    }
}