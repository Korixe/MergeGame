using UnityEngine;

public class PixelArtProgress
{
    public PixelArtData pixelArtData;
    public bool isUnlocked;
    public bool isCompleted;
    public bool[] isPixelColored;

    public PixelArtProgress(PixelArtData pixelArtData)
    {
        this.pixelArtData = pixelArtData;
        isUnlocked = false;
        isCompleted = false;
        isPixelColored = new bool[pixelArtData.width * pixelArtData.height];
    }
}
