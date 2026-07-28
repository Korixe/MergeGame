using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GridManager : MonoBehaviour
{
    public int rows;
    public int columns;
    private GridCell[,] _cells;
    public GameObject cellPrefab;
    public GridLayoutGroup gridLayout;

    private void Awake()
    {
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

                //test
                if (i == 0 && j == 0)
                {
                    _cells[i, j].isTaken = true;
                    _cells[i, j].itemData = ScriptableObject.CreateInstance<ItemData>();
                    _cells[i, j].itemData.itemName = "A";
                    _cells[i, j].itemData.type = 1;
                    _cells[i, j].itemData.level = 1;
                    _cells[i, j].itemData.isMergeable = true;
                    _cells[i, j].itemData.isClickable = true;
                    _cells[i, j].itemData.prefab = Resources.Load<Sprite>("Assets/Prefabs/temp1");

                    ItemView itemView = spawnedObject.AddComponent<ItemView>();
                    itemView.SetItemData(_cells[i, j].itemData);
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
}