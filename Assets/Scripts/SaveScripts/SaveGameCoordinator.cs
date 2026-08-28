using UnityEngine;
using System.Collections.Generic;

public class SaveGameCoordinator : MonoBehaviour
{
    public static SaveGameCoordinator Instance;

    private List<ISaveable> _saveables = new List<ISaveable>();
    private SaveGameData _loadedData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _loadedData = SaveManager.LoadGame();
    }

    public void RegisterSaveable(ISaveable saveable)
    {
        if (!_saveables.Contains(saveable))
            _saveables.Add(saveable);

        if(_loadedData != null)
            saveable.LoadSaveData(_loadedData);
        else
            saveable.InitializeNewGame();
    }

    public void UnregisterSaveable(ISaveable saveable)
    {
        _saveables.Remove(saveable);
    }

    public void SaveGame()
    {
        SaveGameData data = _loadedData ?? new SaveGameData();

        foreach (ISaveable saveable in _saveables)
            saveable.CollectSaveData(data);

        SaveManager.SaveGame(data);
        _loadedData = data;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
