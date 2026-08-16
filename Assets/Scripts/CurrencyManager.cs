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
        LoadCurrency();
    }

    private void LoadCurrency()
    {
        // load
        currencyAmount = 10;
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
