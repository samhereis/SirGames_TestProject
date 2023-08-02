using System;
using UnityEngine;

namespace Events
{
    [Serializable]
    public class EventWithOneParameters<T>
    {
        public Action<T> onInvoke { get; private set; }

        [field: SerializeField] public string eventName { get; private set; }
        [field: SerializeField] public int currentSubscribedObjectsCount { get; private set; }
        [field: SerializeField] public int timesInvoked { get; private set; }

        public EventWithOneParameters(string eventName)
        {
            this.eventName = eventName;
        }

        public void AddListener(Action<T> action)
        {
            onInvoke += action;
            UpdateCurrentSubscribedObjectsCount();
        }

        public void RemoveListener(Action<T> action)
        {
            onInvoke -= action;
            UpdateCurrentSubscribedObjectsCount();
        }

        public void Clear()
        {
            onInvoke = null;
        }

        public void Invoke(T parameter)
        {
            onInvoke?.Invoke(parameter);
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