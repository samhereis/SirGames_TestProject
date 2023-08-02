using DG.Tweening;
using Identifiers;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Displayers
{
    public class HealthDisplayer : MonoBehaviour
    {
        [SerializeField] private IdentifierBase _identifier;
        [SerializeField] private Slider _slider;

        [Header("Settings")]
        [SerializeField] private Gradient _healthGradient;

        private IHealth _health;
        private Image _fillImageForGradient;
        private Camera _camera;

        private void Start()
        {
            _health = _identifier.GetComponent<IHealth>();

            if (_health != null)
            {
                _health.onHealthChanged += OnHealthChanged;
            }

            if (_slider != null)
            {
                _slider.maxValue = _health.maxHealth;
                _slider.value = _health.currentHealth;
                _fillImageForGradient = _slider.fillRect.GetComponent<Image>();
                _fillImageForGradient.color = _healthGradient.Evaluate(_health.currentHealth / _health.maxHealth);
            }

            _camera = Camera.main;
        }

        private void Update()
        {
            if (_camera != null)
            {
                _slider.transform.rotation = _camera.transform.rotation;
            }
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.onHealthChanged -= OnHealthChanged;
            }
        }

        private void OnHealthChanged(float healthValue)
        {
            _slider.DOValue(healthValue, 0.5f);
            _fillImageForGradient.color = _healthGradient.Evaluate(_health.currentHealth / _health.maxHealth);
        }
    }
}