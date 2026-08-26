using UnityEngine;

public interface ISaveable
{
    void CollectSaveData(SaveGameData saveData);
    void LoadSaveData(SaveGameData saveData);
    void InitializeNewGame();
}
