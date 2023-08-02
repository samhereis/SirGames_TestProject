using System;
using System.Collections.Generic;
using UnityEngine;

namespace Identifiers
{
    public class IdentifierBase : MonoBehaviour
    {
        private Dictionary<Type, Component> _components = new Dictionary<Type, Component>();

        public T TryGet<T>() where T : Component
        {
            T component = null;

            if (_components.ContainsKey(typeof(T)))
            {
                component = _components[typeof(T)] as T;
            }
            else
            {
                component = GetComponentInChildren<T>(true);
                if (component != null) _components.Add(typeof(T), component);
            }

            return component;
        }

        public bool TryGet<T>(out T result) where T : Component
        {
            result = TryGet<T>();
            return result == null;
        }
    }
}
