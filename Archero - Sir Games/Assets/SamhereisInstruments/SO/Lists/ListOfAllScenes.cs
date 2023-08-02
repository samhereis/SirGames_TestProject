using DataClasses;
using Interfaces;
using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllScenes", menuName = "Scriptables/Lists/ListOfAllScenes")]
    public class ListOfAllScenes : ScriptableObject, IInitializable
    {
        [field: SerializeField] public List<AScene> _scenes { get; private set; } = new List<AScene>();

        [Header("Debug")]
        [SerializeField] private int _currentGameSceneIndex = 0;

        public void Initialize()
        {
            UpdateCurrentSceneData();
        }

        private void UpdateCurrentSceneData()
        {
            var gameSave = GameSaveManager.GetGameSave();

            _currentGameSceneIndex = gameSave.gameSceneIndex;
            Debug.Log($"Current Game Scene Index: {_currentGameSceneIndex}");
        }

        public void IncreaseLevelIndex()
        {
            GameSave gameSave = GameSaveManager.GetGameSave();

            var sceneIndex = _currentGameSceneIndex + 1;

            if (sceneIndex >= _scenes.Count)
            {
                sceneIndex = 0;
            }

            gameSave.gameSceneIndex = sceneIndex;

            GameSaveManager.SaveGameSave(gameSave);

            Debug.Log("Saved Level Index: " + gameSave.gameSceneIndex);

            UpdateCurrentSceneData();
        }

        public AScene GetCurrentGameScene()
        {
            var sceneIndex = _currentGameSceneIndex;

            if (sceneIndex >= _scenes.Count)
            {
                sceneIndex = 0;
            }

            return _scenes[sceneIndex];
        }

        public List<AScene> GetScenes()
        {
            return _scenes;
        }
    }
}