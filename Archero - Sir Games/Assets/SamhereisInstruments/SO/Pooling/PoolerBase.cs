using Helpers;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Pooling
{
    public abstract class PoolerBase<T> : ScriptableObject, IInitializable<Transform>, IInitializable where T : Component
    {
        [field: SerializeField] public T poolable { get; protected set; }
        [SerializeField] protected Queue<T> _poolablesQueue = new Queue<T>();
        [SerializeField] protected List<T> _poolablesDequeued = new List<T>();
        [SerializeField] protected List<T> _poolablesQueued = new List<T>();

        [Header("Settings")]
        [SerializeField] protected bool _setParent = true;
        [SerializeField] private int _defaultSpawnQuantity = 2;

        [Header("Debug")]
        [SerializeField] private int _spawnCount = 0;
        [SerializeField] private Transform _parent;

        public virtual void Initialize(Transform parent)
        {
            _parent = parent;
        }

        public virtual async void Initialize()
        {
            Clear();
            await SpawnAsync(_defaultSpawnQuantity, _parent);
        }

        public virtual async Task SpawnAsync(int quantity = 5, Transform parent = null)
        {
            for (int i = 0; i < quantity; i++)
            {
                await AsyncHelper.Delay();

                var poolableInstance = Instantiate(poolable, parent);
                poolableInstance.gameObject.name += _spawnCount;

                PutIn(poolableInstance);

                _spawnCount++;
            }
        }

        public virtual void Spawn(int quantity = 5, Transform parent = null)
        {
            for (int i = 0; i < quantity; i++)
            {
                var poolableInstance = Instantiate(poolable, parent);
                poolableInstance.gameObject.name += _spawnCount;

                PutIn(poolableInstance);

                _spawnCount++;
            }
        }

        [ContextMenu(nameof(Clear))]
        public void Clear()
        {
            _poolablesQueue?.Clear();
            _poolablesQueue = new Queue<T>();

            _poolablesDequeued?.Clear();
            _poolablesDequeued = new List<T>();

            _poolablesQueued?.Clear();
            _poolablesQueued = new List<T>();

            _spawnCount = 0;
        }

        public virtual T PutOff(Transform position, Quaternion rotation, Transform parent = null)
        {
            return PutOff(position.position, rotation, parent);
        }

        public virtual T PutOff(Transform position, Transform parent = null)
        {
            return PutOff(position.position, Quaternion.identity, parent);
        }

        public virtual T PutOff(Vector3 position, Transform parent = null)
        {
            return PutOff(position, Quaternion.identity, parent);
        }

        public virtual T PutOff(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            T poolable;

            if (_poolablesQueue.Count < 1) Spawn(_defaultSpawnQuantity, _parent);

            poolable = _poolablesQueue.Dequeue();

            _poolablesDequeued.SafeAdd(poolable);
            _poolablesQueued.SafeRemove(poolable);

            if (parent != null) poolable.transform.parent = parent;

            poolable.gameObject.SetActive(true);

            poolable.transform.position = position;
            poolable.transform.rotation = rotation;

            return poolable;
        }

        public virtual Task<T> PutOffAsync(Transform position, Quaternion rotation, Transform parent = null)
        {
            return PutOffAsync(position.position, rotation, parent);
        }

        public virtual Task<T> PutOffAsync(Transform position, Transform parent = null)
        {
            return PutOffAsync(position.position, Quaternion.identity, parent);
        }

        public virtual Task<T> PutOffAsync(Vector3 position, Transform parent = null)
        {
            return PutOffAsync(position, Quaternion.identity, parent);
        }

        public virtual async Task<T> PutOffAsync(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            T poolable;

            if (_poolablesQueue.Count < 1) await SpawnAsync(_defaultSpawnQuantity, _parent);

            poolable = _poolablesQueue.Dequeue();

            _poolablesDequeued.SafeAdd(poolable);
            _poolablesQueued.SafeRemove(poolable);

            if (parent != null) poolable.transform.parent = parent;

            poolable.gameObject.SetActive(true);

            poolable.transform.position = position;
            poolable.transform.rotation = rotation;

            return poolable;
        }

        public virtual void PutIn(T poolable)
        {
            if (poolable)
            {
                try
                {
                    _poolablesQueue?.Enqueue(poolable);
                    _poolablesDequeued?.SafeRemove(poolable);
                    _poolablesQueued?.SafeAdd(poolable);

                    poolable.gameObject.SetActive(false);

                    if (_setParent) poolable.transform.parent = _parent;
                }
                finally
                {

                }
            }
        }

        public virtual async void PutInAll()
        {
            var copy = new List<T>();
            copy.AddRange(_poolablesDequeued);

            foreach (var poolable in copy)
            {
                PutIn(poolable);
                await AsyncHelper.Delay();
            }

            Clear();
        }
    }
}