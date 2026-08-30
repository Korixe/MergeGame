using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PixelArtDatabase", menuName = "Scriptable Objects/PixelArtDatabase")]
public class PixelArtDatabase : ScriptableObject
{
    public List<PixelArtData> pixelArtData;

    public PixelArtData GetPixelArtByID(string id)
    {
        return pixelArtData.Find(pixelArt => pixelArt.pixelArtID == id);
    }
}
