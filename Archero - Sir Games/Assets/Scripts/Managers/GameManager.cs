using Characters.Enemy;
using DataClasses;
using DI;
using Events;
using Helpers;
using Identifiers;
using InGameStrings;
using Interfaces;
using SO.Lists;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UI.Canvases;
using UnityEngine;
using Values;

namespace Managers
{
    public class GameManager : MonoBehaviour, IDIDependent, ISubscribesToEvents
    {
        [Header("DI")]
        [DI(DIStrings.onGameStart)][SerializeField] private EventWithNoParameters _onGameStart;
        [DI(DIStrings.isGameActive)][SerializeField] private ValueEvent<bool> _isGameActive;
        [DI(DIStrings.isGamePaused)][SerializeField] private ValueEvent<bool> _isGamePaused;
        [DI(DIStrings.onWin)][SerializeField] private EventWithNoParameters _onWin;
        [DI(DIStrings.onLose)][SerializeField] private EventWithNoParameters _onLose;
        [DI(DIStrings.onAllEnemiesKilled)][SerializeField] private EventWithNoParameters _onAllEnemiesKilled;
        [DI(DIStrings.coinsCount)][SerializeField] private ValueEvent<int> _coinsCount;

        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;

        [Header("Components")]
        [SerializeField] private GameplayWindow _gameplayWindow;
        [SerializeField] private PauseWindow _pauseWindow;
        [SerializeField] private LoseWindow _loseWindow;
        [SerializeField] private EnemySpawner _enemySpawner;

        [Header("Debug")]
        [SerializeField] private PlayerIdentifier _playerIdentifier;
        [SerializeField] private List<EnemyIdentifier> _enemyIdentifiers = new List<EnemyIdentifier>();

        GameSave _gameSave = new GameSave();

        private bool _isActive = false;

        private async void Start()
        {
            (this as IDIDependent).LoadDependencies();

            _pauseWindow.Initialize();
            _loseWindow.Initialize();

            _pauseWindow.Disable(0);
            _loseWindow.Disable(0);

            await AsyncHelper.Delay(250);

            _onGameStart.AddListener(OnGameStart);

            _gameplayWindow.Initialize();
            _enemySpawner.Initialize();
            _isGamePaused?.ChangeValue(false);

            var gameSave = GameSaveManager.GetGameSave();
            _coinsCount.ChangeValue(gameSave.coinsCount);

            _isActive = true;
        }

        private void OnDestroy()
        {
            _onGameStart.RemoveListener(OnGameStart);
            UnsubscribeFromEvents();
        }

        private void OnGameStart()
        {
            _playerIdentifier = FindObjectOfType<PlayerIdentifier>(true);
            _enemyIdentifiers = FindObjectsOfType<EnemyIdentifier>(true).ToList();

            SubscribeToEvents();
        }

        public void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _playerIdentifier.TryGet<PlayerHealth>().onDie += OnPlayerDie;

            foreach (var enemyIdentifier in _enemyIdentifiers)
            {
                enemyIdentifier.TryGet<EnemyHealth>().onDie += OnEnemyDie;
            }

            _onWin.AddListener(Win);
            _isGamePaused.AddListener(Pause);
            _coinsCount.AddListener(ChangeCoinsValue);
        }

        public void UnsubscribeFromEvents()
        {
            _playerIdentifier.TryGet<PlayerHealth>().onDie -= OnPlayerDie;

            foreach (var enemyIdentifier in _enemyIdentifiers)
            {
                enemyIdentifier.TryGet<EnemyHealth>().onDie -= OnEnemyDie;
            }

            _onWin.RemoveListener(Win);
            _isGamePaused.RemoveListener(Pause);
            _coinsCount.RemoveListener(ChangeCoinsValue);
            _onGameStart.RemoveListener(OnGameStart);
        }

        private void OnAllEnemiesKilled()
        {
            _onAllEnemiesKilled?.Invoke();
        }

        private async void Win()
        {
            if (_isActive == false) return;
            _isActive = false;

            UnsubscribeFromEvents();

            GameSaveManager.SaveGameSave(_gameSave);

            _isGameActive.ChangeValue(false);

            _listOfAllScenes.IncreaseLevelIndex();
            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.GetCurrentGameScene());
        }

        private void Lose()
        {
            if (_isActive == false) return;

            UnsubscribeFromEvents();

            _isGameActive.ChangeValue(false);
            _onLose?.Invoke();

            _loseWindow?.Open();
        }

        private void Pause(bool isPaused)
        {
            if (_isActive == false) return;

            if (isPaused == true)
            {
                _isGameActive.ChangeValue(false);
                _pauseWindow?.Open();
            }
            else
            {
                _isGameActive.ChangeValue(true);
                _gameplayWindow?.Open();
            }
        }

        private void OnPlayerDie(IHealth health)
        {
            if (_isActive == false) return;

            Lose();
        }

        private void OnEnemyDie(IHealth health)
        {
            if (_isActive == false) return;

            health.onDie -= OnEnemyDie;

            _enemyIdentifiers.Remove(health.damagedGameobject.TryGet<EnemyIdentifier>());

            if (_enemyIdentifiers.Count == 0)
            {
                OnAllEnemiesKilled();
            }
        }

        public void ChangeCoinsValue(int value = 1)
        {
            _gameSave.coinsCount = value;
        }
    }
}