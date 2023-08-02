using DI;
using Events;
using Helpers;
using InGameStrings;
using Interfaces;
using Managers;
using PlayerInputHolder;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.UI;
using Values;

namespace UI.Canvases
{
    public class GameplayWindow : CanvasWindowBase, IDIDependent, IInitializable
    {
        [Header("DI")]
        [DI(DIStrings.inputHolder)][SerializeField] private Input_SO _inputActions;
        [DI(DIStrings.onGameStart)][SerializeField] private EventWithNoParameters _onGameStart;
        [DI(DIStrings.isGameActive)][SerializeField] private ValueEvent<bool> _isGameActive;
        [DI(DIStrings.isGamePaused)][SerializeField] private ValueEvent<bool> _isGamePaused;
        [DI(DIStrings.coinsCount)][SerializeField] private ValueEvent<int> _coinsCount;

        [Header("Popups")]
        [SerializeField] private CountdownPopup _countdownPopup;

        [Header("Elements")]
        [SerializeField] private CanvasGroup _gameplayWindowUIHolder;
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private TextMeshProUGUI _coinsCountText;
        [SerializeField] private Button _pauseButton;

        protected override async void Awake()
        {
            base.Awake();

            _gameplayWindowUIHolder.FadeDownQuick();
            _countdownPopup.Disable(0);
        }

        public async void Initialize()
        {
            (this as IDIDependent).LoadDependencies();

            Enable();

            _countdownPopup.Open();
            await _countdownPopup.Countdown(3);

            _countdownPopup.Close();
            _gameplayWindowUIHolder.FadeUp(0.5f);

            _onGameStart?.Invoke();
            _isGameActive?.ChangeValue(true);
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();

            _inputActions.Enable();
            _inputActions.input.UI.Disable();
            _inputActions.input.Player.Enable();

            OnCoinsCountChanged(_coinsCount.value);

            var gameSave = GameSaveManager.GetGameSave();
            _currentLevelText.text = "Level: " + (gameSave.gameSceneIndex + 1);
        }

        public override void Disable(float? duration = null)
        {
            UnsubscribeFromEvents();
            base.Disable(duration);
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _coinsCount.AddListener(OnCoinsCountChanged);
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _coinsCount.RemoveListener(OnCoinsCountChanged);
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        private void OnPauseButtonClicked()
        {
            _isGamePaused?.ChangeValue(true);
        }

        private void OnCoinsCountChanged(int value)
        {
            _coinsCountText.text = "Coins: " + value;
        }
    }
}