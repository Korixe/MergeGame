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
                spawnedObject.transform.parent = gridLayout.transform;
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