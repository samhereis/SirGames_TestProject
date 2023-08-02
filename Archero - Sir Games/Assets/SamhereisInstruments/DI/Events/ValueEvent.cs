using System;
using UnityEngine;

namespace Values
{
    [Serializable]
    public class ValueEvent<T>
    {
        [field: SerializeField] public T value { get; protected set; }
        [field: SerializeField] public string eventName { get; private set; }
        [field: SerializeField] public int currentSubscribedObjectsCount { get; private set; }
        [field: SerializeField] public int timesInvoked { get; private set; }

        [SerializeField] protected Action<T> onValueChange { get; set; }

        public ValueEvent(string eventName)
        {
            this.eventName = eventName;
        }

        public virtual void AddListener(Action<T> listener)
        {
            onValueChange += listener;
            UpdateCurrentSubscribedObjectsCount();
        }

        public virtual void RemoveListener(Action<T> listener)
        {
            onValueChange -= listener;
            UpdateCurrentSubscribedObjectsCount();
        }

        public virtual void ChangeValue(T sentValue)
        {
            value = sentValue;
            onValueChange?.Invoke(sentValue);

            timesInvoked++;
        }

        private void UpdateCurrentSubscribedObjectsCount()
        {
            if (onValueChange == null)
            {
                currentSubscribedObjectsCount = 0;
            }
            else
            {
                currentSubscribedObjectsCount = onValueChange.GetInvocationList().Length;
            }
        }
    }
}