using DI;
using Helpers;
using InGameStrings;
using SO.Lists;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvases
{
    public class MainMenuWindow : CanvasWindowBase, IDIDependent
    {
        [Header("DI")]
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;

        [Header("Buttons")]
        [SerializeField] private Button _playButton;

        protected override async void Awake()
        {
            base.Awake();

            Disable(0);

            while (BindDIScene.isInjected == false)
            {
                await AsyncHelper.Delay();
            }

            (this as IDIDependent).LoadDependencies();

            Open();
        }

        public override void Enable(float? duration = null)
        {
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

            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }

        private async void OnPlayButtonClicked()
        {
            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.GetCurrentGameScene());
        }
    }
}