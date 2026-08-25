using TMPro;
using UnityEngine;
using System;

public class EnergyManager : MonoBehaviour, ISaveable
{
    public static EnergyManager Instance;
    public event Action<int> OnEnergyChanged;
    public TextMeshProUGUI energyText;
    private int _energyAmount;
    private int _maxEnergy = 100;
    private float _energyRegenTime = 5f; // 300 seconds
    private float _currentRegenerationTime;
    private long _lastSyncTime;
    public int energyAmount => _energyAmount;
    public long lastSyncTime => _lastSyncTime;

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
        _currentRegenerationTime = _energyRegenTime;
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

    public void CollectSaveData(SaveGameData data)
    {
        data.savedEnergyAmount = _energyAmount;
        data.savedLastSyncTime = _lastSyncTime;
    }

    public void LoadSaveData(SaveGameData data)
    {
        RestoreEnergyState(data.savedEnergyAmount, data.savedLastSyncTime);
    }

    public void SyncTime()
    {
        _lastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public void RestoreEnergyState(int savedEnergy, long savedSyncTime)
    {
        _energyAmount = savedEnergy;
        _lastSyncTime = savedSyncTime;
        
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long elapsedTime = currentTime - _lastSyncTime;

        if (_energyAmount < _maxEnergy && elapsedTime > 0)
        {
            int energyToRegenerate = (int)(elapsedTime / (long)_energyRegenTime);
            long remainder = elapsedTime % (long)_energyRegenTime;

            _energyAmount += energyToRegenerate;

            if (_energyAmount >= _maxEnergy)
            {
                _energyAmount = _maxEnergy;
                _currentRegenerationTime = _energyRegenTime;
                SyncTime();
            }
            else
            {
                _currentRegenerationTime = _energyRegenTime - remainder;
                _lastSyncTime = currentTime - remainder;
            }
        }
        else if (_energyAmount >= _maxEnergy)
            SyncTime();

        NotifyEnergyChanged();
    }

    public void InitializeNewGame()
    {
        _energyAmount = _maxEnergy;
        SyncTime();
        NotifyEnergyChanged();
    }

    public void RegenerateEnergy()
    {
        if (_energyAmount < _maxEnergy)
        {
            _energyAmount++;
            NotifyEnergyChanged();
            SyncTime();
        }
    }

    public void AddEnergy(int amount)
    {
        _energyAmount += amount;
        NotifyEnergyChanged();
    }

    public bool SubtractEnergy(int amount)
    {
        if (_energyAmount < amount)
            return false;

        _energyAmount -= amount;
        NotifyEnergyChanged();

        if (_energyAmount == _maxEnergy - amount)
        {
            _currentRegenerationTime = _energyRegenTime;
            SyncTime();
        }
        
        return true;
    }

    private void NotifyEnergyChanged()
    {
        OnEnergyChanged?.Invoke(_energyAmount);
        if (energyText != null)
            energyText.text = _energyAmount.ToString();
    }
}
