using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite prefab;
    public int type;
    public int level;
    public bool isMergeable;
    public bool isClickable;

}
