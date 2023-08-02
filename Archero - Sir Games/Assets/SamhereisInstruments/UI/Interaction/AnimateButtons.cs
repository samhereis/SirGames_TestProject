using DG.Tweening;
using Helpers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Interaction
{
    [DisallowMultipleComponent]
    public class AnimateButtons : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private float _onOverScale = 1.1f;
        [SerializeField] private float _normaleScale = 1;

        [Header("Timing")]
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private float _delayBetweenAnimations = 0.1f;

        [Header("Events")]
        [SerializeField] private AnimateButtonsEvents _events;

        public async void OnPointerEnter(PointerEventData eventData)
        {
            await AsyncHelper.Delay(_delayBetweenAnimations);
            transform.DOScale(_onOverScale, _animationDuration).SetEase(Ease.InOutBack);

            _events._onHover?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _events._onClick?.Invoke();
        }

        public async void OnPointerExit(PointerEventData eventData)
        {
            await AsyncHelper.Delay(_delayBetweenAnimations);
            transform.DOScale(_normaleScale, _animationDuration).SetEase(Ease.InOutBack);

            _events._onExit?.Invoke();
        }
    }

    [System.Serializable]
    internal class AnimateButtonsEvents
    {
        [SerializeField] internal UnityEvent _onHover = new UnityEvent();
        [SerializeField] internal UnityEvent _onClick = new UnityEvent();
        [SerializeField] internal UnityEvent _onExit = new UnityEvent();
    }
}