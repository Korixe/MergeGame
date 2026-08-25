using UnityEngine;
using System.Collections.Generic;

public class SaveGameCoordinator : MonoBehaviour
{
    public static SaveGameCoordinator Instance;

    public List<MonoBehaviour> saveableComponents;
    private List<ISaveable> _saveables;

    private void Awake()
    {
        Instance = this;
        _saveables = new List<ISaveable>();

        foreach (MonoBehaviour component in saveableComponents)
        {
            if (component is ISaveable saveable)
                _saveables.Add(saveable);
            else
                Debug.LogWarning(component.name + "does not implement ISaveable;");
        }
    }

    private void Start()
    {
        SaveGameData loadedData = SaveManager.LoadGame();
        if (loadedData != null)
        {
            foreach (ISaveable saveable in _saveables)
                saveable.LoadSaveData(loadedData);
        }
        else
        {
            StartNewGame();
        }
    }

    public void StartNewGame()
    {
        CurrencyManager.Instance.SetCurrency(0);
        EnergyManager.Instance.InitializeNewGame();
        GridManager.Instance.SpawnTestItems();
    }

    public void SaveGame()
    {
        SaveGameData data = new SaveGameData();

        foreach (ISaveable saveable in _saveables)
            saveable.CollectSaveData(data);

        SaveManager.SaveGame(data);
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
