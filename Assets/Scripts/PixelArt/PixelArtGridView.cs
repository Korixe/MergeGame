using UnityEngine;

public class PixelArtGridView : MonoBehaviour
{
    public PixelArtData pixelArtData;
    public PixelCellView[,] pixelCells;

    private void InitializeGrid()
    {
        pixelCells = new PixelCellView[pixelArtData.width, pixelArtData.height];

        for (int i = 0; i < pixelArtData.width; i++)
        {
            for (int j = 0; j < pixelArtData.height; j++)
                pixelCells[i, j] = new PixelCellView(i, j);
        }
    }
}
