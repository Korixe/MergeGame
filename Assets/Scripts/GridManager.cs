using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    
    public int rows;
    public int columns;
    private GridCell[,] _cells;
    public GameObject cellPrefab;
    public GameObject itemPrefab;
    public GridLayoutGroup gridLayout;
    public ItemData[] itemDatas;

    private void Awake()
    {
        Instance = this;
        InitializeGrid();
        
        SaveGameData loadedData = SaveManager.LoadGame();   
        if (loadedData != null)
            RestoreFromLoadedData(loadedData);     
        else
            SpawnTestItems();
    }

    private void InitializeGrid()
    {
        _cells = new GridCell[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                _cells[i, j] = new GridCell(i, j);
                GameObject spawnedObject = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity);
                spawnedObject.transform.SetParent(gridLayout.transform, false);
                CellView cellView = spawnedObject.GetComponent<CellView>();
                cellView.SetPosition(i, j);
                _cells[i, j].cellView = cellView;
            }
        }
    }

    private void SpawnTestItems()
    {
        //test
        if (IsCellFree(3, 2)) SpawnItemInCell(GetCell(3, 2), GetCell(3, 2).cellView, itemDatas[2]);
    }


    public GridCell GetCell(int row, int col)
    {
        if (row < 0 || row >= rows || col < 0 || col >= columns)
            return null;

        return _cells[row, col];
    }

    public bool IsCellFree(int row, int col)
    {
        GridCell cell = GetCell(row, col);
        return cell != null && !cell.isTaken;
    }

    public void SpawnItemInCell(GridCell cell, CellView cellView, ItemData itemData)
    {
        cell.isTaken = true;
        cell.itemData = itemData;
        cell.cellView = cellView;

        GameObject spawnedItem = Instantiate(itemPrefab, cellView.transform);
        ItemView itemView = spawnedItem.GetComponent<ItemView>();
        itemView.SetItemData(itemData);
        cell.itemView = itemView;
    }

    public ItemData GetItemDataByID(string id)
    {
        foreach (ItemData data in itemDatas)
        {
            if (data.itemID == id)
                return data;
        }
        return null;
    }

    public SaveGameData CollectSaveData()
    {
        SaveGameData saveData = new SaveGameData();
        saveData.savedCellData = new System.Collections.Generic.List<SaveCellData>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GridCell cell = _cells[i, j];
                if (cell.isTaken && cell.itemData != null)
                {
                    SaveCellData cellData = new SaveCellData
                    {
                        row = cell.row,
                        column = cell.column,
                        ItemID = cell.itemData.itemID,
                        itemUsed = cell.itemView.itemUsed,
                        isOnCooldown = cell.itemView.isOnCooldown
                    };

                    saveData.savedCellData.Add(cellData);
                    saveData.savedCurrencyAmount = CurrencyManager.Instance.currencyAmount;
                }
            }
        }

        return saveData;
    }

    public void RestoreFromLoadedData(SaveGameData data)
    {
        foreach (SaveCellData cellData in data.savedCellData)
        {
            GridCell cell = GetCell(cellData.row, cellData.column);
            ItemData itemData = GetItemDataByID(cellData.ItemID);
            SpawnItemInCell(cell, cell.cellView, itemData);

            if(itemData is GeneratorData)
            {
                cell.itemView.RestoreGeneratorState(cellData.itemUsed, cellData.isOnCooldown);
            }
            
            CurrencyManager.Instance.SetCurrency(data.savedCurrencyAmount);
        }
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveManager.SaveGame(CollectSaveData());
    }

    public void OnApplicationQuit()
    {
        SaveManager.SaveGame(CollectSaveData());
    }
}