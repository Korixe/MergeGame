using UnityEngine;
using System.IO;

public class SaveManager
{
    private static string saveFilePath = Application.persistentDataPath + "/savegame.json";

    public static void SaveGame(SaveGameData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to " + saveFilePath);
    }

    public static SaveGameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveGameData saveData = JsonUtility.FromJson<SaveGameData>(json);
            Debug.Log("Game loaded from " + saveFilePath);
            return saveData;
        }
        else
        {
            Debug.LogWarning("No save file found at " + saveFilePath);
            return null;
        }
    }
}
