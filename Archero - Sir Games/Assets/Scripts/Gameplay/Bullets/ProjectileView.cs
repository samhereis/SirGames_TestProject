using Helpers;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _flash;
        [SerializeField] private ParticleSystem _hit;

        private void OnValidate()
        {
            foreach (var trans in GetComponentsInChildren<Transform>(true))
            {
                trans.localScale = Vector3.one;
                trans.localPosition = Vector3.zero;
                trans.localEulerAngles = Vector3.zero;
            }
        }

        public void Init()
        {
            _hit.Stop();
            _hit.gameObject.SetActive(false);

            _flash.gameObject.SetActive(true);
            _flash.Play();
        }

        public async void OnEnd(Action callback = null)
        {
            _hit.Stop();
            _hit.gameObject.SetActive(false);

            _hit.gameObject.SetActive(true);
            _hit.Play();

            if (callback != null)
            {
                await AsyncHelper.Delay(_hit.main.duration);
                callback.Invoke();
            }
        }

        public async Task OnEndAsync(Action callback = null)
        {
            _hit.Stop();
            _hit.gameObject.SetActive(false);

            _hit.gameObject.SetActive(true);
            _hit.Play();

            if (callback != null)
            {
                await AsyncHelper.Delay(_hit.main.duration);
                callback.Invoke();
            }
        }
    }
}