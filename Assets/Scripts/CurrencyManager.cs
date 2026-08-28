using TMPro;
using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour, ISaveable
{
    public static CurrencyManager Instance;
    public event Action<int> OnCurrencyChanged;
    private int _currencyAmount;
    public int currencyAmount => _currencyAmount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SaveGameCoordinator.Instance.RegisterSaveable(this);
    }

    public void CollectSaveData(SaveGameData data)
    {
        data.savedCurrencyAmount = _currencyAmount;
    }

    public void LoadSaveData(SaveGameData data)
    {
        SetCurrency(data.savedCurrencyAmount);
    }

    public void InitializeNewGame()
    {
        SetCurrency(0);
    }

    public void SetCurrency(int amount)
    {
        _currencyAmount = amount;
        OnCurrencyChanged?.Invoke(_currencyAmount);
    }

    public void AddCurrency(int amount)
    {
        SetCurrency(_currencyAmount + amount);
    }

    public bool SubtractCurrency(int amount)
    {
        if (_currencyAmount < amount)
            return false;

        SetCurrency(_currencyAmount - amount);
        return true;
    }
}
