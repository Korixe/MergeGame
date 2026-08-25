using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CurrencyManager : MonoBehaviour, ISaveable
{
    public static CurrencyManager Instance;
    public TextMeshProUGUI currencyText;
    private int _currencyAmount;
    public int currencyAmount => _currencyAmount;

    private void Awake()
    {
        Instance = this;
        UpdateCurrencyText();
    }

    public void CollectSaveData(SaveGameData data)
    {
        data.savedCurrencyAmount = _currencyAmount;
    }

    public void LoadSaveData(SaveGameData data)
    {
        SetCurrency(data.savedCurrencyAmount);
    }

    public void SetCurrency(int amount)
    {
        _currencyAmount = amount;
        UpdateCurrencyText();
    }

    public void AddCurrency(int amount)
    {
        _currencyAmount += amount;
        UpdateCurrencyText();
    }

    public bool SubtractCurrency(int amount)
    {
        if (_currencyAmount < amount)
            return false;
            
        _currencyAmount -= amount;
        UpdateCurrencyText();
        return true;
    }

    private void UpdateCurrencyText()
    {
        currencyText.text = _currencyAmount.ToString();
    }

}
