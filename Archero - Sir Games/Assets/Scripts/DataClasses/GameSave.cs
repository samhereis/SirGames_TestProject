using System;

namespace DataClasses
{
    [Serializable]
    public record GameSave : ISavable
    {
        public int gameSceneIndex = 0;
        public int coinsCount = 0;
    }
}