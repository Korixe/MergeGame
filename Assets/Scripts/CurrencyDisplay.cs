using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CurrencyManager.Instance.OnCurrencyChanged += UpdateText;
        UpdateText(CurrencyManager.Instance.currencyAmount);
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateText;
    }

    private void UpdateText(int amount)
    {
        _text.text = amount.ToString();
    }
}