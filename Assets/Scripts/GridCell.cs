[System.Serializable]
public class GridCell
{
    public int row;
    public int column;
    public bool isTaken;

    public GridCell(int row, int column)
    {
        this.row = row;
        this.column = column;
        this.isTaken = false;
    }
}