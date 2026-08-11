using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite sprite;
    public int type;
    public int level;
    public bool isMergeable;
    public bool isClickable;
    public ItemData nextLevelItemData;

}
