[System.Serializable]
public class GridCell
{
    public int row;
    public int column;
    public bool isTaken;
    public ItemData itemData;
    public ItemView itemView;

    public GridCell(int row, int column)
    {
        this.row = row;
        this.column = column;
        isTaken = false;
        itemData = null;
        itemView = null;
    }
}