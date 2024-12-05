using System.Runtime.InteropServices;
using UnityEngine;

namespace Utils.LocalStorageSaving
{
    public static class LocalStorageHandler
    {
        [DllImport("__Internal")]
        private static extern void SaveData(string key, string value);

        [DllImport("__Internal")]
        private static extern string LoadData(string key);

        public static void SaveGame(string json)
        {
            SaveData("userData", json);
            Debug.Log("Game saved to localStorage: " + json);
        }

        public static string LoadGame()
        {
            var json = LoadData("userData");
            Debug.Log("Game loaded from localStorage: " + json);
            return json;
        }
    }
}