using DataClasses;
using Helpers;

namespace Managers
{
    public class GameSaveManager
    {
        private const string _gameSaveFolder = "GameSaves";
        private const string _gameSaveFile = "GameSave";

        public static GameSave GetGameSave()
        {
            var gameSave = SaveHelper.GetStoredDataClass<GameSave>(_gameSaveFolder, _gameSaveFile);

            if (gameSave == null)
            {
                gameSave = new GameSave();
            }

            return gameSave;
        }

        public static void SaveGameSave(GameSave gameSave)
        {
            SaveHelper.SaveToJson(gameSave, _gameSaveFolder, _gameSaveFile);
        }
    }
}