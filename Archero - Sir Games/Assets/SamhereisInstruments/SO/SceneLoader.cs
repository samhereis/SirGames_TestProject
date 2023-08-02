using DataClasses;
using Helpers;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Tools
{
    [CreateAssetMenu(fileName = "Scene Loader", menuName = "Scriptables/Helpers/Scene Loader")]
    public class SceneLoader : ScriptableObject
    {
        public readonly UnityEvent<AScene> onSceneStartLoading = new UnityEvent<AScene>();

        [SerializeField] private bool _loading = false;

        public async Task LoadSceneAsync(AScene aScene, Action<float> onUpdate = null)
        {
            if (_loading == false)
            {
                _loading = true;

                onSceneStartLoading?.Invoke(aScene);

                var asyncOperation = SceneManager.LoadSceneAsync(aScene.sceneCode, LoadSceneMode.Single);

                while (asyncOperation.isDone == false)
                {
                    onUpdate?.Invoke(asyncOperation.progress);
                    await AsyncHelper.Delay();

                    if (asyncOperation.progress >= 0.9f)
                    {
                        Time.timeScale = 1;
                    }
                }

                _loading = false;
            }
        }

        public async void LoadMainMenu()
        {
            if (_loading == false)
            {
                _loading = true;

                var asyncOperation = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);

                while (asyncOperation.isDone == false)
                {
                    await AsyncHelper.Delay();

                    if (asyncOperation.progress >= 0.9f)
                    {
                        Time.timeScale = 1;
                    }
                }

                _loading = false;
            }
        }

        public void LoadSceneAdditively(int sceneId)
        {
            SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
        }
    }
}