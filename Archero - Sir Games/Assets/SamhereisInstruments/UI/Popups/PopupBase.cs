using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace UI.Popups
{
    public abstract class PopupBase : MonoBehaviour
    {
        protected static Action<PopupBase> _onAPopupOpen;

        [SerializeField] protected BaseSettings _baseSettings = new BaseSettings();

        private CancellationTokenSource _onDestroyCancellationTokenSource = new CancellationTokenSource();

        protected virtual void Awake()
        {
            _onAPopupOpen += OnAPopupOpen;
        }

        protected virtual void OnDestroy()
        {
            _onAPopupOpen -= OnAPopupOpen;
            _onDestroyCancellationTokenSource.Cancel();
        }

        public virtual async void OnAPopupOpen(PopupBase popup)
        {
            if (popup != this) Disable();
        }

        public void Open()
        {
            Enable();
        }

        public void Close()
        {
            Disable();
        }

        public virtual void Enable(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration;
            if (_baseSettings.notifyOthers == true) _onAPopupOpen?.Invoke(this);

            if (_baseSettings.scalesObjects.Count > 0)
            {
                transform.localScale = Vector3.one;
                if (_baseSettings.enableDisable) gameObject.SetActive(true);

                foreach (var item in _baseSettings.scalesObjects)
                {
                    item.DOKill();
                    item.DOScale(1, duration.Value);
                }
            }
            else
            {
                if (_baseSettings.enableDisable) gameObject.SetActive(true);
                transform.DOKill();
                transform.DOScale(1, duration.Value);
            }
        }

        public virtual void Disable(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration;

            if (duration.Value == 0)
            {
                transform.localScale = Vector3.zero;

                foreach (var item in _baseSettings.scalesObjects)
                {
                    item.localScale = Vector3.zero;
                }
            }

            if (_baseSettings.scalesObjects.Count > 0)
            {
                foreach (var item in _baseSettings.scalesObjects)
                {
                    item.DOKill();
                    item.DOScale(0, duration.Value);
                }

                if (_onDestroyCancellationTokenSource.IsCancellationRequested == false)
                {
                    transform.localScale = Vector3.zero;
                    if (_baseSettings.enableDisable) gameObject.SetActive(false);
                }
            }
            else
            {
                transform.DOKill();
                transform.DOScale(0, duration.Value);

                if (_onDestroyCancellationTokenSource.IsCancellationRequested == false)
                {
                    if (_baseSettings.enableDisable) gameObject.SetActive(false);
                }
            }
        }

        [System.Serializable]
        protected class BaseSettings
        {
            [Header("Settings")]
            public bool enableDisable = true;
            public bool notifyOthers = true;
            public float animationDuration = 0.5f;

            public List<Transform> scalesObjects = new List<Transform>();
        }
    }
}