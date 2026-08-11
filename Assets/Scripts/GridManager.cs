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
    [SerializeField] private ItemData[] _itemDatas;

    private void Awake()
    {
        Instance = this;
        InitializeGrid();
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

                //test
                if (i == 0 && j == 0 || i == 1 && j == 1)
                {
                    SpawnItemInCell(_cells[i, j], cellView, _itemDatas[0]);
                }

                if (i == 2 && j == 2)
                {
                    SpawnItemInCell(_cells[i, j], cellView, _itemDatas[1]);
                }

                if (i == 3 && j == 2)
                {
                    SpawnItemInCell(_cells[i, j], cellView, _itemDatas[2]);
                }

                if (i == 4 && j == 2)
                {
                    SpawnItemInCell(_cells[i, j], cellView, _itemDatas[2]);
                }
            }
        }
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
}