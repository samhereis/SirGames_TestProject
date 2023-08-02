using DG.Tweening;
using Helpers;
using Interfaces;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvases
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class CanvasWindowBase : MonoBehaviour, IUIWindow
    {
        public static Action<CanvasWindowBase> onAWindowOpen { get; private set; }

        [SerializeField] protected BaseSettings _baseSettings = new BaseSettings();

        protected CancellationTokenSource _onDestroyCTS = new CancellationTokenSource();

        protected virtual void Awake()
        {
            Setup();
            onAWindowOpen += OnAWindowOpen;
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeFromEvents();
            onAWindowOpen -= OnAWindowOpen;

            _onDestroyCTS.Cancel();
        }


        public virtual void OnAWindowOpen(IUIWindow uIWIndow)
        {
            if (uIWIndow != this as IUIWindow)
            {
                Disable();
            }
        }

        public virtual void Open()
        {
            Enable();
        }

        public virtual void Close()
        {
            Disable();
        }

        public virtual void Exit()
        {
            Disable();
            _baseSettings.openOnExit?.Open();
        }

        public virtual void Enable(float? duration = null)
        {
            Debug.Log("Opennings: " + gameObject.name);

            if (_baseSettings.notifyOthers == true) onAWindowOpen?.Invoke(this);

            _baseSettings.canvasGroup.DOKill();

            if (duration == null) duration = _baseSettings.animationDuration_Enable;

            if (_onDestroyCTS.IsCancellationRequested) return;

            if (_baseSettings.enableDisable) gameObject.SetActive(true);
            _baseSettings.canvasGroup.FadeUp(duration.Value);
        }

        public virtual void Disable(float? duration = null)
        {
            Debug.Log("Closing: " + gameObject.name);

            _baseSettings.canvasGroup.DOKill();

            if (duration == null) duration = _baseSettings.animationDuration_Disable;

            if (duration.Value == 0)
            {
                _baseSettings.canvasGroup.alpha = 0;
                if (_baseSettings.enableDisable) gameObject.SetActive(false);
            }
            else
            {
                _baseSettings.canvasGroup.FadeDown(duration.Value)?.OnComplete(() =>
                {
                    if (_baseSettings.enableDisable) gameObject.SetActive(false);
                });
            }
        }

        protected virtual void SubscribeToEvents()
        {
            UnsubscribeFromEvents();
        }

        protected virtual void UnsubscribeFromEvents()
        {

        }

        public virtual void Setup()
        {
            try
            {
                if (_baseSettings.canvasGroup == null) _baseSettings.canvasGroup = GetComponent<CanvasGroup>();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        [Serializable]
        protected class BaseSettings
        {
            [Header("Settings")]
            public bool enableDisable = true;
            public bool notifyOthers = true;
            public float animationDuration_Enable = 1;
            public float animationDuration_Disable = 0.25f;

            [Header("Components")]
            public CanvasGroup canvasGroup;

            [Header("Optional")]
            public CanvasWindowBase openOnExit;
        }
    }
}