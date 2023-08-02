using System;
using UnityEngine;

namespace DataClasses
{
    [Serializable]
    public class AScene
    {
        [field: SerializeField] public string sceneName { get; private set; }
        [field: SerializeField] public string sceneCode { get; private set; }

        public string GetSceneName()
        {
            return sceneName;
        }
    }
}