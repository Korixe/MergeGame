using System.Collections.Generic;

[System.Serializable]
public class SaveGameData
{
    public List<SaveCellData> savedCellData;
    public int savedCurrencyAmount;
    public int savedEnergyAmount;
    public long savedLastSyncTime;
}
