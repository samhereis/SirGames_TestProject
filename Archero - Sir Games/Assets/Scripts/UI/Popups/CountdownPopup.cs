using DG.Tweening;
using Helpers;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI.Popups
{
    public class CountdownPopup : PopupBase
    {
        [Header("Elemets")]
        [SerializeField] private TextMeshProUGUI _countdownText;

        protected override async void Awake()
        {
            base.Awake();
        }

        public async Task Countdown(int seconds)
        {
            _countdownText.text = seconds.ToString();

            for (int i = seconds; i > 0; i--)
            {
                _countdownText.DOKill();
                _countdownText.transform.DOScale(1, 0.25f).OnComplete(async () =>
                {
                    await AsyncHelper.Delay(500);
                    _countdownText.transform.DOScale(0.25f, 0.25f);
                });

                _countdownText.text = i.ToString();

                await AsyncHelper.Delay(1000);

            }
        }
    }
}