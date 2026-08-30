using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PixelArtManager : MonoBehaviour
{
    public static PixelArtManager Instance;
    public PixelArtDatabase pixelArtDatabase;
    public GridLayoutGroup gridLayout;
    public List<PixelArtProgress> pixelArtProgressList;

    public void PurchasePixelArt(string pixelArtID)
    {
        PixelArtData pixelArtData = pixelArtDatabase.GetPixelArtByID(pixelArtID);
        if (pixelArtData != null && CurrencyManager.Instance.SubtractCurrency(pixelArtData.price))
        {
            PixelArtProgress progress = new PixelArtProgress(pixelArtData)
            {
                isUnlocked = true,
                isCompleted = false
            };
            pixelArtProgressList.Add(progress);
        }
        else
            Debug.LogWarning("Pixel art not found");
    }

    public void ColorPixel()
    {
        // color pixel
    }
}
