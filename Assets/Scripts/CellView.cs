using UnityEngine;

public class CellView : MonoBehaviour
{
    public int row;
    public int column;

    public void SetPosition(int newRow, int newCol)
    {
        row = newRow;
        column = newCol;
    }
}
