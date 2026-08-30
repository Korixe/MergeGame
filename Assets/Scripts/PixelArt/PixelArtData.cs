using UnityEngine;

[CreateAssetMenu(fileName = "PixelArtData", menuName = "Scriptable Objects/PixelArtData")]
public class PixelArtData : ScriptableObject
{
    public string pixelArtID;
    public string pixelArtName;
    public int width;
    public int height;
    public Color[] pixelColors;
    public int price;
    public Sprite spritePrev;
    public bool isClickable;
}
