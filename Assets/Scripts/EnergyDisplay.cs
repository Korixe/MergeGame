using TMPro;
using UnityEngine;

public class EnergyDisplay : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        EnergyManager.Instance.OnEnergyChanged += UpdateText;
        UpdateText(EnergyManager.Instance.energyAmount);
    }

    private void OnDestroy()
    {
        if (EnergyManager.Instance != null)
            EnergyManager.Instance.OnEnergyChanged -= UpdateText;
    }

    private void UpdateText(int amount)
    {
        _text.text = amount.ToString();
    }
}