using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class GridManager : MonoBehaviour, ISaveable
{
    public static GridManager Instance;
    
    public int rows;
    public int columns;
    private GridCell[,] _cells;
    public GameObject cellPrefab;
    public GameObject itemPrefab;
    public GridLayoutGroup gridLayout;
    public ItemDatabase itemDatabase;

    private void Awake()
    {
        Instance = this;
        InitializeGrid();
    }

    public void CollectSaveData(SaveGameData data)
    {
        data.savedCellData = new List<SaveCellData>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GridCell cell = _cells[i, j];
                if (cell.isTaken && cell.itemData != null)
                {
                    data.savedCellData.Add(new SaveCellData
                    {
                        row = cell.row,
                        column = cell.column,
                        ItemID = cell.itemData.itemID,
                        itemUsed = cell.itemView.itemUsed,
                        isOnCooldown = cell.itemView.isOnCooldown
                    });
                }
            }
        }
    }

    public void LoadSaveData(SaveGameData data)
    {
        foreach (SaveCellData cellData in data.savedCellData)
        {
            GridCell cell = GetCell(cellData.row, cellData.column);
            if (cell == null)
            {
                Debug.LogWarning("Saved cell position out of range: " + cellData.row + "," + cellData.column);
                continue;
            }

            ItemData itemData = itemDatabase.GetItemByID(cellData.ItemID);
            if (itemData == null)
            {
                Debug.LogWarning("Item with id " + cellData.ItemID + " not found");
                continue;
            }

            SpawnItemInCell(cell, cell.cellView, itemData);

            if(itemData is GeneratorData)
                cell.itemView.RestoreGeneratorState(cellData.itemUsed, cellData.isOnCooldown);
        }
    }

    public void SpawnTestItems()
    {
        //test
        if (IsCellFree(3, 2)) SpawnItemInCell(GetCell(3, 2), GetCell(3, 2).cellView, itemDatabase.GetItemByID("test_item_lvl2"));
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

    public List<GridCell> GetCellsWithItem(ItemData itemData, int amount)
    {
        List<GridCell> cellsWithItem = new List<GridCell>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GridCell cell = _cells[i, j];
                if (cell.isTaken && cell.itemData == itemData)
                {
                    cellsWithItem.Add(cell);
                    if (cellsWithItem.Count >= amount)
                        return cellsWithItem;
                }
            }
        }

        return null; // not found
    }

    public void RemoveItemFromCell(GridCell cell)
    {
        if (cell.isTaken)
        {
            Destroy(cell.itemView.gameObject);
            cell.isTaken = false;
            cell.itemData = null;
            cell.itemView = null;
        }
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
}