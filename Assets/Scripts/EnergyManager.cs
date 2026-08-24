using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;
    public TextMeshProUGUI energyText;
    private int _energyAmount;
    private int _maxEnergy = 100;
    private float _energyRegenTime = 300f; // 300 seconds
    private float _currentRegenerationTime;
    private long _lastSyncTime;
    public int energyAmount => _energyAmount;
    public long lastSyncTime => _lastSyncTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _currentRegenerationTime = _energyRegenTime;
        OfflineEnergyRegeneration();
    }

    private void Update()
    {
        if (_energyAmount < _maxEnergy)
        {
            _currentRegenerationTime -= Time.deltaTime;

            if (_currentRegenerationTime <= 0f)
            {
                RegenerateEnergy();
                _currentRegenerationTime = _energyRegenTime;
            }
        }
    }

    public void SyncTime()
    {
        _lastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    private void OfflineEnergyRegeneration()
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long elapsedTime = currentTime - _lastSyncTime;

        int energyToRegenerate = (int)(elapsedTime / _energyRegenTime);
        if (energyToRegenerate > 0)
        {
            OfflineAddEnergy(energyToRegenerate);
            SyncTime();
        }
        UpdateEnergyText();
    }

    public void SetEnergy(int amount)
    {
        _energyAmount = amount;
        UpdateEnergyText();
    }

    public void RegenerateEnergy()
    {
        if (_energyAmount < _maxEnergy)
        {
            _energyAmount++;
            UpdateEnergyText();
            SyncTime();
        }
    }

    public void OfflineAddEnergy(int amount)
    {
        _energyAmount += amount;
        if (_energyAmount > _maxEnergy)
            _energyAmount = _maxEnergy;

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

        if (_energyAmount == _maxEnergy - amount)
        {
            _currentRegenerationTime = _energyRegenTime;
            SyncTime();
        }
        
        return true;
    }

    private void UpdateEnergyText()
    {
        energyText.text = _energyAmount.ToString();
    }

}
