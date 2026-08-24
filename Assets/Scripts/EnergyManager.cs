using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;
    public TextMeshProUGUI energyText;
    private int _energyAmount;
    private int _maxEnergy = 100;
    private float _energyRegenTime = 300f; 
    public int energyAmount => _energyAmount;

    private void Awake()
    {
        Instance = this;
        UpdateEnergyText();
    }

    public void SetEnergy(int amount)
    {
        _energyAmount = amount;
        UpdateEnergyText();
    }  

    public void AddEnergy(int amount)
    {
        _energyAmount += amount;
        UpdateEnergyText();
    }

    public bool SubtractEnergy(int amount)
    {
        if (_energyAmount < amount)
            return false;

        _energyAmount -= amount;
        UpdateEnergyText();
        return true;
    }

    private void UpdateEnergyText()
    {
        energyText.text = _energyAmount.ToString();
    }

}
