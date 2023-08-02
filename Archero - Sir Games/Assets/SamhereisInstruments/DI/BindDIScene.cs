using Events;
using InGameStrings;
using Interfaces;
using Managers;
using System.Collections.Generic;
using UnityEngine;
using Values;

namespace DI
{
    public class BindDIScene : MonoBehaviour
    {
        public static bool isInjected { get; private set; } = false;

        [SerializeField] private List<MonoBehaviourToDI> _objects = new List<MonoBehaviourToDI>();
        [SerializeField] private List<SOToDI> _scriptableObjects = new List<SOToDI>();
        [SerializeField] private List<EventToDI> _eventsWithNoParameters = new List<EventToDI>();

        private bool _wasInjected = false;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            if (isInjected == true)
            {
                _wasInjected = true;

                Destroy(gameObject);
                return;
            }

            DIBox.Clear();

            foreach (var obj in _objects)
            {
                if (obj.Instance is IInitializable)
                {
                    (obj.Instance as IInitializable).Initialize();
                }

                DIBox.Add(obj.Instance, obj.id);
            }

            foreach (var obj in _scriptableObjects)
            {
                if (obj.Instance is IInitializable)
                {
                    (obj.Instance as IInitializable).Initialize();
                }

                DIBox.Add(obj.Instance, obj.id);
            }

            foreach (var obj in _eventsWithNoParameters)
            {
                obj.Initialize();
                DIBox.Add(obj.Instance, obj.id);
            }

            InjectEventsWithParameters();
            InjecValueEvents();

            isInjected = true;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (_wasInjected == false) isInjected = false;
        }

        private void InjectEventsWithParameters()
        {

        }

        private void InjecValueEvents()
        {
            var coinsCount = GameSaveManager.GetGameSave();

            var coinsCountValueEvent = new ValueEvent<int>(DIStrings.coinsCount);
            coinsCountValueEvent.ChangeValue(coinsCount.coinsCount);

            DIBox.Add(coinsCountValueEvent, DIStrings.coinsCount);

            var isGameActiveVE = new ValueEvent<bool>(DIStrings.isGameActive);
            DIBox.Add(isGameActiveVE, DIStrings.isGameActive);

            var isGamePausedVE = new ValueEvent<bool>(DIStrings.isGamePaused);
            DIBox.Add(isGamePausedVE, DIStrings.isGamePaused);
        }

        [System.Serializable]
        public class MonoBehaviourToDI
        {
            public string id = "";
            public MonoBehaviour Instance;
        }

        [System.Serializable]
        public class SOToDI
        {
            public string id = "";
            public ScriptableObject Instance;
        }

        [System.Serializable]
        public class EventToDI : IInitializable
        {
            public string id = "";
            public EventWithNoParameters Instance;

            public void Initialize()
            {
                Instance = new EventWithNoParameters(id);
            }
        }
    }
}