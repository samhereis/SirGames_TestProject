using Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LazyUpdators
{
    [CreateAssetMenu(fileName = "LazyUpdator", menuName = "Scriptables/LazyUpdator")]
    public class LazyUpdator_SO : ScriptableObject
    {
        private List<Func<Task>> _tasks = new List<Func<Task>>();
        [SerializeField] private int _tasksCount = 0;
        [SerializeField] private bool _isRunning = false;

        public void AddToQueue(Func<Task> task)
        {
            if (_tasks.Count == 0)
            {
                _isRunning = false;
                DoLazyUpdateWithAwait(Add);
            }
            else Add();

            void Add() { _tasks.SafeAdd(task); }

            _tasksCount = _tasks.Count;
        }

        public void RemoveFromQueue(Func<Task> task)
        {
            _tasks.SafeRemove(task);

            _tasksCount = _tasks.Count;
        }

        private void RemoveAllNullTasks()
        {
            _tasks.RemoveNulls();
        }

        private async void DoLazyUpdateWithAwait(Action doBeforeUpdate = null)
        {
            if (_isRunning) return;

            doBeforeUpdate?.Invoke();

            try
            {
                while (_tasks.Count > 0 && Application.isPlaying)
                {
                    _isRunning = true;

                    RemoveAllNullTasks();

                    var actions = _tasks.ToArray();
                    foreach (Func<Task> action in actions)
                    {
                        try
                        {
                            if (action == null)
                            {
                                _tasks.Remove(action);
                            }
                            else
                            {
                                await action.Invoke();
                            }
                        }
                        catch { }
                    }
                }
            }
            finally
            {
                _isRunning = false;
            }
        }
    }
}