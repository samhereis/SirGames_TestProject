using DI;
using InGameStrings;
using Interfaces;
using PlayerInputHolder;
using SO.Lists;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvases
{
    public class LoseWindow : CanvasWindowBase, IDIDependent, IInitializable
    {
        [Header("DI")]
        [DI(DIStrings.inputHolder)][SerializeField] private Input_SO _inputActions;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;

        [Header("Elemts")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;

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

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        }

        private void OnMainMenuButtonClicked()
        {
            _sceneLoader.LoadMainMenu();
        }

        private async void OnRestartButtonClicked()
        {
            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.GetCurrentGameScene());
        }
    }
}