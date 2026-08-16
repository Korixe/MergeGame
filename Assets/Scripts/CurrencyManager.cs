using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int currencyAmount;

    private void Awake()
    {
        Instance = this;
        SaveGameData loadedData = SaveManager.LoadGame();   
        if (loadedData != null)
            LoadCurrency(loadedData);
        else
            currencyAmount = 10;
    }

    private void LoadCurrency(SaveGameData loadedData)
    {
        currencyAmount = loadedData.savedCurrencyAmount;
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        SaveGameData saveData = new SaveGameData();
        saveData.savedCurrencyAmount = currencyAmount;
        if (pauseStatus)
            SaveManager.SaveGame(saveData);
    }

    public void OnApplicationQuit()
    {
        SaveGameData saveData = new SaveGameData();
        saveData.savedCurrencyAmount = currencyAmount;
        SaveManager.SaveGame(saveData);
    }

    private void AddCurrency(int amount)
    {
        currencyAmount += amount;
    }

    private void SubtractCurrency(int amount)
    {
        currencyAmount -= amount;
    }

}
