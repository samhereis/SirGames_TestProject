using DI;
using InGameStrings;
using Interfaces;
using PlayerInputHolder;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Values;

namespace UI.Canvases
{
    public class PauseWindow : CanvasWindowBase, IDIDependent, IInitializable
    {
        [Header("DI")]
        [DI(DIStrings.inputHolder)][SerializeField] private Input_SO _inputActions;
        [DI(DIStrings.isGamePaused)][SerializeField] private ValueEvent<bool> _isGamePaused;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;

        [Header("Elemts")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _mainMenuButton;

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void Initialize()
        {
            (this as IDIDependent).LoadDependencies();
        }

        public override void Enable(float? duration = null)
        {
            _inputActions.input.Player.Disable();
            _inputActions.input.UI.Enable();

            base.Enable(duration);

            SubscribeToEvents();
        }

        public override void Disable(float? duration = null)
        {
            UnsubscribeFromEvents();

            base.Disable(duration);
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _resumeButton.onClick.AddListener(OnResumeButtonClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        }

        private void OnResumeButtonClicked()
        {
            _isGamePaused?.ChangeValue(false);
        }

        private void OnMainMenuButtonClicked()
        {
            _sceneLoader.LoadMainMenu();
        }
    }
}